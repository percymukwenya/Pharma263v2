using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Pharma263.MVC.Exceptions;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Pharma263.MVC.Middleware
{
    /// <summary>
    /// Global exception handling middleware with correlation ID tracking
    /// Catches all unhandled exceptions and returns standardized error responses
    /// Phase 2.3: Error Handling Standardization
    /// </summary>
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Generate correlation ID for request tracing
            var correlationId = context.Request.Headers["X-Correlation-ID"].ToString();
            if (string.IsNullOrEmpty(correlationId))
            {
                correlationId = Guid.NewGuid().ToString();
                context.Request.Headers.Add("X-Correlation-ID", correlationId);
            }

            // Add correlation ID to response headers
            context.Response.Headers.Add("X-Correlation-ID", correlationId);

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, correlationId);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception, string correlationId)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            var errorResponse = new ErrorResponse
            {
                CorrelationId = correlationId,
                Timestamp = DateTime.UtcNow
            };

            switch (exception)
            {
                case ValidationException validationEx:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse.Message = "Validation failed";
                    errorResponse.Errors = validationEx.Errors;
                    _logger.LogWarning(exception,
                        "Validation error occurred. CorrelationId: {CorrelationId}, Errors: {Errors}",
                        correlationId, validationEx.GetFormattedErrors());
                    break;

                case EntityNotFoundException notFoundEx:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    errorResponse.Message = exception.Message;
                    errorResponse.Detail = $"Entity: {notFoundEx.EntityType}, ID: {notFoundEx.EntityId}";
                    _logger.LogWarning(exception,
                        "Entity not found. CorrelationId: {CorrelationId}, Entity: {EntityType}, ID: {EntityId}",
                        correlationId, notFoundEx.EntityType, notFoundEx.EntityId);
                    break;

                case BusinessRuleViolationException businessEx:
                    response.StatusCode = (int)HttpStatusCode.Conflict;
                    errorResponse.Message = exception.Message;
                    errorResponse.Detail = $"Rule: {businessEx.RuleName}";
                    _logger.LogWarning(exception,
                        "Business rule violation. CorrelationId: {CorrelationId}, Rule: {RuleName}",
                        correlationId, businessEx.RuleName);
                    break;

                case InsufficientStockException stockEx:
                    response.StatusCode = (int)HttpStatusCode.Conflict;
                    errorResponse.Message = exception.Message;
                    errorResponse.Detail = $"Medicine: {stockEx.MedicineName}, Requested: {stockEx.RequestedQuantity}, Available: {stockEx.AvailableQuantity}";
                    _logger.LogWarning(exception,
                        "Insufficient stock. CorrelationId: {CorrelationId}, Medicine: {MedicineName}, Requested: {RequestedQuantity}, Available: {AvailableQuantity}",
                        correlationId, stockEx.MedicineName, stockEx.RequestedQuantity, stockEx.AvailableQuantity);
                    break;

                case ApiException apiEx:
                    response.StatusCode = (int)apiEx.StatusCode;
                    errorResponse.Message = "External API call failed";
                    errorResponse.Detail = apiEx.Message;
                    _logger.LogError(exception,
                        "API call failed. CorrelationId: {CorrelationId}, StatusCode: {StatusCode}, URL: {RequestUrl}",
                        correlationId, apiEx.StatusCode, apiEx.RequestUrl);
                    break;

                case UnauthorizedAccessException:
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    errorResponse.Message = "Unauthorized access";
                    errorResponse.Detail = exception.Message;
                    _logger.LogWarning(exception,
                        "Unauthorized access attempt. CorrelationId: {CorrelationId}",
                        correlationId);
                    break;

                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorResponse.Message = "An unexpected error occurred. Please contact support.";
                    errorResponse.Detail = "Internal server error";

                    // Log full exception details for internal errors
                    _logger.LogError(exception,
                        "Unhandled exception occurred. CorrelationId: {CorrelationId}, Message: {Message}, StackTrace: {StackTrace}",
                        correlationId, exception.Message, exception.StackTrace);
                    break;
            }

            // Add instance path for problem details
            errorResponse.Instance = context.Request.Path;

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };

            var result = JsonSerializer.Serialize(errorResponse, options);
            await response.WriteAsync(result);
        }
    }

    /// <summary>
    /// Standardized error response format
    /// Follows RFC 7807 Problem Details for HTTP APIs
    /// </summary>
    public class ErrorResponse
    {
        public string Message { get; set; }
        public string Detail { get; set; }
        public string Instance { get; set; }
        public string CorrelationId { get; set; }
        public DateTime Timestamp { get; set; }
        public object Errors { get; set; }
    }
}

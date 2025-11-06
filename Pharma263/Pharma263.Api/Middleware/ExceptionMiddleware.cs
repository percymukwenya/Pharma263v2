using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Pharma263.Application.Exceptions;
using System.Net;
using System.Threading.Tasks;
using System;
using Pharma263.Api.Models;
using Newtonsoft.Json;

namespace Pharma263.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment _environment;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IWebHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
        {
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            CustomProblemDetails problem = new();
            var isDevelopment = _environment.IsDevelopment();

            switch (ex)
            {
                case BadRequestException badRequestException:
                    statusCode = HttpStatusCode.BadRequest;
                    problem = new CustomProblemDetails
                    {
                        Title = badRequestException.Message,
                        Status = (int)statusCode,
                        Detail = isDevelopment ? badRequestException.InnerException?.Message : null,
                        Type = nameof(BadRequestException),
                        Errors = badRequestException.ValidationErrors
                    };
                    break;
                case NotFoundException NotFound:
                    statusCode = HttpStatusCode.NotFound;
                    problem = new CustomProblemDetails
                    {
                        Title = NotFound.Message,
                        Status = (int)statusCode,
                        Type = nameof(NotFoundException),
                        Detail = isDevelopment ? NotFound.InnerException?.Message : null,
                    };
                    break;
                default:
                    problem = new CustomProblemDetails
                    {
                        Title = isDevelopment ? ex.Message : "An error occurred processing your request.",
                        Status = (int)statusCode,
                        Type = nameof(HttpStatusCode.InternalServerError),
                        Detail = isDevelopment ? ex.StackTrace : null,
                    };
                    break;
            }

            httpContext.Response.StatusCode = (int)statusCode;

            // Always log full exception details for monitoring (not sent to client in production)
            _logger.LogError(ex, "An error occurred: {ErrorMessage}. Status Code: {StatusCode}", ex.Message, statusCode);

            await httpContext.Response.WriteAsJsonAsync(problem);
        }
    }
}

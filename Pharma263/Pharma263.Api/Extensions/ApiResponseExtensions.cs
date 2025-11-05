using Microsoft.AspNetCore.Mvc.ModelBinding;
using Pharma263.Domain.Common;
using System.Collections.Generic;
using System.Linq;

namespace Pharma263.Api.Extensions
{
    public static class ApiResponseExtensions
    {
        public static ApiResponse<T> CreateValidationFailure<T>(string message, ModelStateDictionary modelState, int statusCode = 400)
        {
            var errors = modelState
                .SelectMany(x => x.Value.Errors)
                .Select(x => x.ErrorMessage)
                .ToList();

            return ApiResponse<T>.CreateFailure(message, statusCode, errors);
        }
    }
}
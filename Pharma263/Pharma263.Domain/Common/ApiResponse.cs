using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Pharma263.Domain.Common
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }

        public T Data { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Message { get; set; }

        public int StatusCode { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<string> Errors { get; set; }

        public static ApiResponse<T> CreateSuccess(T data = default, string message = "Operation successful", int statusCode = 200)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Data = data,
                Message = message,
                StatusCode = statusCode
            };
        }

        public static ApiResponse<T> CreateFailure(string message, int statusCode = 400, List<string> errors = null)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                StatusCode = statusCode,
                Errors = errors
            };
        }

    }
}

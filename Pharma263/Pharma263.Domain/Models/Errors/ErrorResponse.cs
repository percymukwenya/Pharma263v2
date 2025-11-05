using System.Text.Json.Serialization;

namespace Pharma263.Domain.Models.Errors
{
    public class ErrorResponse
    {
        [JsonPropertyName("code")]
        public int ErrorCode { get; set; }

        [JsonPropertyName("message")]
        public string ErrorMessage { get; set; }

        public ErrorResponse()
        {
        }

        public ErrorResponse(int errorCode, string errorMessage)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }
    }
}

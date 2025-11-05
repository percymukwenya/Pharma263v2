using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Pharma263.Integration.Api.Common
{
    public class ErrorResponse
    {
        [JsonProperty(PropertyName = "code")]
        [JsonPropertyName("code")]
        public int ErrorCode { get; set; }

        [JsonProperty(PropertyName = "message")]
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

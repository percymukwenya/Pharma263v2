using System.Text.Json.Serialization;

namespace Pharma263.Domain.Models.Errors
{
    public class ModelStateError
    {
        [JsonPropertyName("field")]
        public string Property { get; set; }

        [JsonPropertyName("error")]
        public string Message { get; set; }
    }
}

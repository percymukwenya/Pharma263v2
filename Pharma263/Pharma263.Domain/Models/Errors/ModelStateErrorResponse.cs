using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Pharma263.Domain.Models.Errors
{
    public class ModelStateErrorResponse : ErrorResponse
    {
        [JsonPropertyName("errors")]
        public List<ModelStateError> ModelErrors { get; set; }

        public ModelStateErrorResponse()
        {
        }

        public ModelStateErrorResponse(int errorCode, string errorMessage, List<ModelStateError> errors = null)
            : base(errorCode, errorMessage)
        {
            ModelErrors = errors;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace Pharma263.MVC.Exceptions
{
    /// <summary>
    /// Exception thrown when validation fails
    /// Contains detailed validation errors for multiple fields
    /// </summary>
    public class ValidationException : Exception
    {
        public IDictionary<string, string[]> Errors { get; }

        public ValidationException()
            : base("One or more validation errors occurred.")
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(IDictionary<string, string[]> errors)
            : base("One or more validation errors occurred.")
        {
            Errors = errors;
        }

        public ValidationException(string propertyName, string errorMessage)
            : base($"Validation failed for '{propertyName}': {errorMessage}")
        {
            Errors = new Dictionary<string, string[]>
            {
                { propertyName, new[] { errorMessage } }
            };
        }

        public ValidationException(string message)
            : base(message)
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(string message, Exception innerException)
            : base(message, innerException)
        {
            Errors = new Dictionary<string, string[]>();
        }

        /// <summary>
        /// Gets a formatted error message with all validation errors
        /// </summary>
        public string GetFormattedErrors()
        {
            if (Errors == null || !Errors.Any())
                return Message;

            var errorMessages = Errors.SelectMany(e =>
                e.Value.Select(v => $"{e.Key}: {v}"));

            return string.Join(Environment.NewLine, errorMessages);
        }
    }
}

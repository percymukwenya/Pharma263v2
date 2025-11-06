using System.Collections.Generic;
using System.Linq;

namespace Pharma263.Application.Models
{
    /// <summary>
    /// Represents the result of a validation operation
    /// </summary>
    public class ValidationResult
    {
        public bool IsValid { get; private set; }
        public List<string> Errors { get; private set; }

        public ValidationResult()
        {
            IsValid = true;
            Errors = new List<string>();
        }

        public void AddError(string errorMessage)
        {
            IsValid = false;
            Errors.Add(errorMessage);
        }

        public void AddErrors(IEnumerable<string> errorMessages)
        {
            if (errorMessages != null && errorMessages.Any())
            {
                IsValid = false;
                Errors.AddRange(errorMessages);
            }
        }

        public void Merge(ValidationResult other)
        {
            if (other != null && !other.IsValid)
            {
                IsValid = false;
                Errors.AddRange(other.Errors);
            }
        }

        public static ValidationResult Success() => new ValidationResult();

        public static ValidationResult Failure(string errorMessage)
        {
            var result = new ValidationResult();
            result.AddError(errorMessage);
            return result;
        }

        public static ValidationResult Failure(IEnumerable<string> errorMessages)
        {
            var result = new ValidationResult();
            result.AddErrors(errorMessages);
            return result;
        }
    }
}

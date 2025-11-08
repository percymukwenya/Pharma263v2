using System;

namespace Pharma263.MVC.Exceptions
{
    /// <summary>
    /// Exception thrown when a business rule is violated
    /// Example: Cannot delete a customer with active orders
    /// </summary>
    public class BusinessRuleViolationException : Exception
    {
        public string RuleName { get; }
        public object AdditionalData { get; }

        public BusinessRuleViolationException(string message)
            : base(message)
        {
        }

        public BusinessRuleViolationException(string message, string ruleName)
            : base(message)
        {
            RuleName = ruleName;
        }

        public BusinessRuleViolationException(string message, string ruleName, object additionalData)
            : base(message)
        {
            RuleName = ruleName;
            AdditionalData = additionalData;
        }

        public BusinessRuleViolationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}

using Pharma263.Domain.Enums;
using Pharma263.Domain.Models.Errors;
using System;

namespace Pharma263.Domain.Exceptions
{
    public class DomainException : PharmaBaseException
    {
        public DomainException() : base() { }

        public DomainException(ErrorEnum errorEnum, string message)
            : base(errorEnum, message) { }

        public DomainException(ModelStateErrorResponse errorContent, string message)
            : base(errorContent, message) { }

        public DomainException(ModelStateErrorResponse errorContent, string message, Exception inner)
            : base(errorContent, message, inner) { }
    }
}

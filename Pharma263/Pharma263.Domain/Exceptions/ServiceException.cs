using Pharma263.Domain.Enums;
using Pharma263.Domain.Models.Errors;
using System;

namespace Pharma263.Domain.Exceptions
{
    public class ServiceException : PharmaBaseException
    {
        public ServiceException() : base() { }

        public ServiceException(ErrorEnum errorEnum, string message)
            : base(errorEnum, message) { }

        public ServiceException(ModelStateErrorResponse errorContent, string message)
            : base(errorContent, message) { }

        public ServiceException(ModelStateErrorResponse errorContent, string message, Exception inner)
            : base(errorContent, message, inner) { }
    }
}

using Pharma263.Domain.Enums;
using Pharma263.Domain.Models.Errors;
using System;

namespace Pharma263.Domain.Exceptions
{
    [Serializable]
    public class PharmaBaseException : Exception
    {
        public ModelStateErrorResponse ErrorContent { get; set; }

        public PharmaBaseException()
        {            
        }

        public PharmaBaseException(ErrorEnum errorEnum, string message)
            : base(message)
        {
            ArgumentNullException.ThrowIfNull(message);
            ErrorContent = new ModelStateErrorResponse((int)errorEnum, errorEnum.ToString());
        }

        public PharmaBaseException(ModelStateErrorResponse errorContent, string message)
            : base(message)
        {
            ArgumentNullException.ThrowIfNull(errorContent);
            ArgumentNullException.ThrowIfNull(message);
            ErrorContent = errorContent;
        }

        public PharmaBaseException(ModelStateErrorResponse errorContent, string message, Exception inner)
            : base(message, inner)
        {
            ArgumentNullException.ThrowIfNull(errorContent);
            ArgumentNullException.ThrowIfNull(message);
            ArgumentNullException.ThrowIfNull(inner);
            ErrorContent = errorContent;
        }
    }
}

using System;
using System.Net.Http;

namespace Pharma263.Integration.Api.Common
{
    public class ApiException : Exception
    {
        public HttpMethod HttpMethod { get; set; }
        public string MethodName { get; set; }
        public HttpRequestMessage HttpRequest { get; set; }
        public Type HttpRequestType { get; set; }
        public dynamic HttpRequestContent { get; set; }
        public HttpResponseMessage HttpResponse { get; set; }
        public Type HttpResponseErrorType { get; set; }
        public dynamic HttpResponseContent { get; set; }
        public int ErrorCode { get; set; }
        public Type ErrorContentType { get; set; }
        public object ErrorContent { get; set; }

        public ApiException(int errorCode, string message)
             : base(message)
        {
            ErrorCode = errorCode;
        }

        public ApiException(int errorCode, string message, System.Exception innerException)
             : base(message, innerException)
        {
            ErrorCode = errorCode;
        }

        public ApiException(int errorCode, string message, Type errorContentType, object errorContent = null)
             : base(message)
        {
            ErrorCode = errorCode;
            ErrorContent = errorContent;
            ErrorContentType = errorContentType;
        }
    }
}

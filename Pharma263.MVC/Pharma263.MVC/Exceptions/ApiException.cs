using System;
using System.Net;

namespace Pharma263.MVC.Exceptions
{
    /// <summary>
    /// Exception thrown when an API call fails
    /// Contains HTTP status code and response details
    /// </summary>
    public class ApiException : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public string ResponseContent { get; }
        public string RequestUrl { get; }

        public ApiException(
            string message,
            HttpStatusCode statusCode,
            string requestUrl = null,
            string responseContent = null)
            : base(message)
        {
            StatusCode = statusCode;
            RequestUrl = requestUrl;
            ResponseContent = responseContent;
        }

        public ApiException(
            string message,
            HttpStatusCode statusCode,
            Exception innerException)
            : base(message, innerException)
        {
            StatusCode = statusCode;
        }

        public override string ToString()
        {
            var baseString = base.ToString();
            var details = $"\nHTTP Status: {(int)StatusCode} ({StatusCode})";

            if (!string.IsNullOrEmpty(RequestUrl))
                details += $"\nRequest URL: {RequestUrl}";

            if (!string.IsNullOrEmpty(ResponseContent))
                details += $"\nResponse: {ResponseContent}";

            return baseString + details;
        }
    }
}

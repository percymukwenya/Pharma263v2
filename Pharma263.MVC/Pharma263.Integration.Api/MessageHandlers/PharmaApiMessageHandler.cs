using Pharma263.Integration.Api.Common;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Pharma263.Integration.Api.MessageHandlers
{
    public class PharmaApiMessageHandler : DelegatingHandler
    {
        private const string ERROR_HEADER = "{0} Failed.";
        private const string ERROR_HEADER_HTTP_REQUEST = "HttpRequest: {0}";
        private const string ERROR_HEADER_HTTP_REQUEST_CONTENT = "HttpRequest.Content: {0}";
        private const string ERROR_HEADER_HTTP_RESPONSE = "HttpResponse: {0}";
        private const string ERROR_HEADER_HTTP_RESPONSE_CONTENT = "HttpResponse.Content: {0}";

        protected string BuildErrorMessage(string methodName, HttpRequestMessage httpRequest, string httpRequestContent, HttpResponseMessage httpResponse = null, string httpResponseContent = null)
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(methodName))
            {
                stringBuilder.AppendLine(string.Format(ERROR_HEADER, methodName));
            }

            if (httpRequest != null)
            {
                stringBuilder.AppendLine(string.Format(ERROR_HEADER_HTTP_REQUEST, httpRequest));
            }

            if (!string.IsNullOrWhiteSpace(httpRequestContent))
            {
                stringBuilder.AppendLine(string.Format(ERROR_HEADER_HTTP_REQUEST_CONTENT, httpRequestContent));
            }

            if (httpResponse != null)
            {
                stringBuilder.AppendLine(string.Format(ERROR_HEADER_HTTP_RESPONSE, httpResponse));
            }

            if (!string.IsNullOrWhiteSpace(httpResponseContent))
            {
                stringBuilder.AppendLine(string.Format(ERROR_HEADER_HTTP_RESPONSE_CONTENT, httpResponseContent));
            }

            return stringBuilder.ToString();
        }

        protected async Task HandleErrorResponsesAsync<TException>(string methodName, HttpRequestMessage httpRequest, HttpResponseMessage httpResponse) where TException : ApiException
        {
            var status = (int)httpResponse.StatusCode;

            if (status >= 400 || status == 0 || !(status >= 200 && status <= 204))
            {
                string httpRequestContent = httpRequest.Content != null ? await httpRequest.Content?.ReadAsStringAsync() : null;
                string httpResponseContent = httpResponse.Content != null ? await httpResponse.Content.ReadAsStringAsync() : null;

                if (status == 400 || status == 401)
                {
                    throw (TException)new ApiException(status, BuildErrorMessage(methodName, httpRequest, httpRequestContent, httpResponse, httpResponseContent), typeof(ErrorResponse), httpResponseContent);
                }

                throw (TException)new ApiException(status, BuildErrorMessage(methodName, httpRequest, httpRequestContent, httpResponse, httpResponseContent));
            }
        }

        protected async Task<TResponse> HandleHttpRequestExceptionAsync<TResponse, TException>(string methodName, HttpRequestMessage httpRequest, Func<Task<TResponse>> httpRequestFunction) where TException : ApiException
        {
            try
            {
                return await httpRequestFunction();
            }
            catch (HttpRequestException hre)
            {
                string httpRequestContent = httpRequest.Content != null ? await httpRequest.Content.ReadAsStringAsync() : null;

                throw (TException)new ApiException(0, BuildErrorMessage(methodName, httpRequest, httpRequestContent), hre);
            }
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return await HandleHttpRequestExceptionAsync<HttpResponseMessage, ApiException>("", request, async () =>
            {
                HttpResponseMessage httpResponse = await base.SendAsync(request, cancellationToken);

                await HandleErrorResponsesAsync<ApiException>("", request, httpResponse);

                return httpResponse;
            });
        }
    }
}

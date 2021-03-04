using System;
using System.Net;
using System.Threading.Tasks;

namespace ApiLibs
{
    public class RequestResponse
    {
        internal readonly Service Service;

        protected RequestResponse(HttpStatusCode code)
        {
            StatusCode = code;
        }

        public RequestResponse(HttpStatusCode statusCode, string statusDescription, string responseUri, string errorMessage, string content, object resp, Request request, Service service)
        {
            StatusCode = statusCode;
            StatusDescription = statusDescription;
            ResponseUri = responseUri;
            ErrorMessage = errorMessage;
            Content = content;
            Resp = resp;
            Request = request;
            Service = service;
        }

        public RequestResponse(RequestResponse resp) : this(resp.StatusCode, resp.StatusDescription, resp.ResponseUri, resp.ErrorMessage, resp.Content, resp.Resp, resp.Request, resp.Service) {}

        public string Message => $"Got {(int)StatusCode}:{StatusDescription} while trying to access \"{ResponseUri}\". {ErrorMessage} \n";

        public HttpStatusCode StatusCode { get; }
        public string StatusDescription { get; }
        public string ResponseUri { get; }
        public string ErrorMessage { get; }
        public string Content { get; }
        public object Resp { get; }
        public Request Request { get; }

        public virtual T Convert<T>() => Service.Convert<T>(Content);
        public Task<RequestResponse> Retry()
        {
            Request.Retries++;
            return Service.HandleRequest(Request);
        }

        public static Func<RequestResponse, Task<RequestResponse>> RetryWhen<T>(int timeout = 100) where T : RequestResponse, new()
        {
            var expectedStatuscode = new T().StatusCode;
            return async (i) =>
            {
                if (expectedStatuscode == i.StatusCode)
                {
                    await Task.Delay(timeout);
                    return await i.Retry();
                }
                else
                {
                    return i;
                }
            };
        }
    }
}

namespace System.Runtime.CompilerServices
{
    public class IsExternalInit { }
}
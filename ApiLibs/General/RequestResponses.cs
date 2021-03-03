using System;
using System.Net;
using System.Threading.Tasks;

namespace ApiLibs
{
    public class RequestResponse
    {
        internal readonly Service Service;

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

        public string Message => $"Got {(int)StatusCode}:{StatusDescription} while trying to access \"{ResponseUri}\". {ErrorMessage} \n";

        public HttpStatusCode StatusCode { get; protected set; }
        public string StatusDescription { get; protected set; }
        public string ResponseUri { get; }
        public string ErrorMessage { get; }
        public string Content { get; }
        public object Resp { get; }
        public Request Request { get; }
    }

    public class GenericBaseRequestResponse {
        internal RequestResponse Response { get; init; }

        public GenericBaseRequestResponse(RequestResponse response, HttpStatusCode statusCode)
        {
            Response = response;
            StatusCode = statusCode;
        }

        public HttpStatusCode StatusCode { get; private set; }
        public string StatusDescription => Response.StatusDescription;
        public string ResponseUri => Response.ResponseUri;
        public string ErrorMessage => Response.ErrorMessage;
        public object Resp => Response.Resp;
        public Request Request => Response.Request;

        public Task<RequestResponse> Retry()
        {
            Request.Retries++;
            return Response.Service.HandleRequest(Request);
        } 
    }

    public class RequestResponse<T> : GenericBaseRequestResponse
    {
        public RequestResponse(RequestResponse response, HttpStatusCode statusCode) : base(response, statusCode)
        {
        }

        public T Content() => Response.Service.Convert<T>(Response.Content);
    }
}

namespace System.Runtime.CompilerServices
{
    public class IsExternalInit { }
}
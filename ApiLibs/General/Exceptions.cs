using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace ApiLibs.General
{
    public class RequestException<T> : InvalidOperationException
    {
        public override string Message => _message;
        private readonly string _message;
        public T Response { get; set; }

        public RequestException(int statusCode, string statusDescription, string responseUri, string errorMessage, string responseContent, T response) : base()
        {
            Response = response;
            _message = $"Got {statusCode.ToString()}:{statusDescription} while trying to access \"{responseUri.ToString()}\". {errorMessage} \n {responseContent}";
        }

        public static RequestException<T> ConvertToException(int statuscode, string statusDescription, string responseuri, string errorMessage, string responseContent, T respone)
        {
            switch(statuscode)
            {
                case 400:
                    return new BadRequestException<T>(statuscode, statusDescription, responseuri, errorMessage, responseContent, respone);
                case 401:
                    return new UnAuthorizedException<T>(statuscode, statusDescription, responseuri, errorMessage, responseContent, respone);
                case 403:
                    return new ForbiddenException<T>(statuscode, statusDescription, responseuri, errorMessage, responseContent, respone);
                case 404:
                    return new PageNotFoundException<T>(statuscode, statusDescription, responseuri, errorMessage, responseContent, respone);
                case 405:
                    return new MethodNotAllowedException<T>(statuscode, statusDescription, responseuri, errorMessage, responseContent, respone);
                case 406:
                    return new NotAcceptableException<T>(statuscode, statusDescription, responseuri, errorMessage, responseContent, respone);
                case 408:
                    return new RequestTimeoutException<T>(statuscode, statusDescription, responseuri, errorMessage, responseContent, respone);
                case 409:
                    return new ConflictException<T>(statuscode, statusDescription, responseuri, errorMessage, responseContent, respone);

                case 500:
                    return new InternalServerErrorException<T>(statuscode, statusDescription, responseuri, errorMessage, responseContent, respone);
                case 501:
                    return new NotImplementedException<T>(statuscode, statusDescription, responseuri, errorMessage, responseContent, respone);
                case 502:
                    return new BadGateWayException<T>(statuscode, statusDescription, responseuri, errorMessage, responseContent, respone);

                default:
                    return new RequestException<T>(statuscode, statusDescription, responseuri, errorMessage, responseContent, respone);
            }
        }
    }

    public class NoInternetException : InvalidOperationException
    {
        public NoInternetException(Exception inner) : base(inner.Message, inner) { }
    }


    public class BadRequestException<T> : RequestException<T>
    {
        public BadRequestException(int statuscode, string statusDescription, string responseuri, string errorMessage, string responseContent, T response) : base(statuscode, statusDescription, responseuri, errorMessage, responseContent, response)
        {
        }
    }

    public class ForbiddenException<T> : RequestException<T>
    {
        public ForbiddenException(int statuscode, string statusDescription, string responseuri, string errorMessage, string responseContent, T response) : base(statuscode, statusDescription, responseuri, errorMessage, responseContent, response)
        {
        }
    }

    public class UnAuthorizedException<T> : RequestException<T>
    {
        public UnAuthorizedException(int statuscode, string statusDescription, string responseuri, string errorMessage, string responseContent, T response) : base(statuscode, statusDescription, responseuri, errorMessage, responseContent, response)
        {
        }
    }


    public class PageNotFoundException<T> : RequestException<T>
    {
        public PageNotFoundException(int statuscode, string statusDescription, string responseuri, string errorMessage, string responseContent, T response) : base(statuscode, statusDescription, responseuri, errorMessage, responseContent, response)
        {
        }
    }

    public class MethodNotAllowedException<T> : RequestException<T>
    {
        public MethodNotAllowedException(int statuscode, string statusDescription, string responseuri, string errorMessage, string responseContent, T response) : base(statuscode, statusDescription, responseuri, errorMessage, responseContent, response)
        {
        }
    }

    internal class NotAcceptableException<T> : RequestException<T>
    {
        public NotAcceptableException(int statusCode, string statusDescription, string responseUri, string errorMessage, string responseContent, T response) : base(statusCode, statusDescription, responseUri, errorMessage, responseContent, response)
        {
        }
    }

    internal class RequestTimeoutException<T> : RequestException<T>
    {
        public RequestTimeoutException(int statusCode, string statusDescription, string responseUri, string errorMessage, string responseContent, T response) : base(statusCode, statusDescription, responseUri, errorMessage, responseContent, response)
        {
        }
    }

    internal class ConflictException<T> : RequestException<T>
    {
        public ConflictException(int statusCode, string statusDescription, string responseUri, string errorMessage, string responseContent, T response) : base(statusCode, statusDescription, responseUri, errorMessage, responseContent, response)
        {
        }
    }

    public class InternalServerErrorException<T> : RequestException<T>
    {
        public InternalServerErrorException(int statusCode, string statusDescription, string responseUri, string errorMessage, string responseContent, T response) : base(statusCode, statusDescription, responseUri, errorMessage, responseContent, response)
        {
        }
    }

    public class NotImplementedException<T> : RequestException<T>
    {
        public NotImplementedException(int statusCode, string statusDescription, string responseUri, string errorMessage, string responseContent, T response) : base(statusCode, statusDescription, responseUri, errorMessage, responseContent, response)
        {
        }
    }

    public class BadGateWayException<T> : RequestException<T>
    {
        public BadGateWayException(int statusCode, string statusDescription, string responseUri, string errorMessage, string responseContent, T response) : base(statusCode, statusDescription, responseUri, errorMessage, responseContent, response)
        {
        }
    }
}

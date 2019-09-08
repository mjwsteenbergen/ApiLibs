using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace ApiLibs.General
{
    public class RequestException : InvalidOperationException
    {
        public override string Message => _message;
        private readonly string _message;
        public object Response { get; set; }

        public RequestException(int statusCode, string statusDescription, string responseUri, string errorMessage, string responseContent, object response) : base()
        {
            Response = response;
            _message = $"Got {statusCode.ToString()}:{statusDescription} while trying to access \"{responseUri.ToString()}\". {errorMessage} \n {responseContent}";
        }

        public static RequestException ConvertToException(int statuscode, string statusDescription, string responseuri, string errorMessage, string responseContent, object response)
        {
            switch(statuscode)
            {
                case 204:
                    return new NoContentRequestException(statuscode, statusDescription, responseuri, errorMessage, responseContent, response);
                case 400:
                    return new BadRequestException(statuscode, statusDescription, responseuri, errorMessage, responseContent, response);
                case 401:
                    return new UnAuthorizedException(statuscode, statusDescription, responseuri, errorMessage, responseContent, response);
                case 403:
                    return new ForbiddenException(statuscode, statusDescription, responseuri, errorMessage, responseContent, response);
                case 404:
                    return new PageNotFoundException(statuscode, statusDescription, responseuri, errorMessage, responseContent, response);
                case 405:
                    return new MethodNotAllowedException(statuscode, statusDescription, responseuri, errorMessage, responseContent, response);
                case 406:
                    return new NotAcceptableException(statuscode, statusDescription, responseuri, errorMessage, responseContent, response);
                case 408:
                    return new RequestTimeoutException(statuscode, statusDescription, responseuri, errorMessage, responseContent, response);
                case 409:
                    return new ConflictException(statuscode, statusDescription, responseuri, errorMessage, responseContent, response);

                case 500:
                    return new InternalServerErrorException(statuscode, statusDescription, responseuri, errorMessage, responseContent, response);
                case 501:
                    return new NotImplementedException(statuscode, statusDescription, responseuri, errorMessage, responseContent, response);
                case 502:
                    return new BadGateWayException(statuscode, statusDescription, responseuri, errorMessage, responseContent, response);

                default:
                    return new RequestException(statuscode, statusDescription, responseuri, errorMessage, responseContent, response);
            }
        }
    }

    public class NoInternetException : InvalidOperationException
    {
        public NoInternetException(Exception inner) : base(inner.Message, inner) { }
    }

    public class NoContentRequestException : RequestException
    {
        public NoContentRequestException(int statuscode, string statusDescription, string responseuri, string errorMessage, string responseContent, object response) : base(statuscode, statusDescription, responseuri, errorMessage, responseContent, response)
        {
        }
    }

    public class BadRequestException : RequestException
    {
        public BadRequestException(int statuscode, string statusDescription, string responseuri, string errorMessage, string responseContent, object response) : base(statuscode, statusDescription, responseuri, errorMessage, responseContent, response)
        {
        }
    }

    public class ForbiddenException : RequestException
    {
        public ForbiddenException(int statuscode, string statusDescription, string responseuri, string errorMessage, string responseContent, object response) : base(statuscode, statusDescription, responseuri, errorMessage, responseContent, response)
        {
        }
    }

    public class UnAuthorizedException : RequestException
    {
        public UnAuthorizedException(int statuscode, string statusDescription, string responseuri, string errorMessage, string responseContent, object response) : base(statuscode, statusDescription, responseuri, errorMessage, responseContent, response)
        {
        }
    }


    public class PageNotFoundException : RequestException
    {
        public PageNotFoundException(int statuscode, string statusDescription, string responseuri, string errorMessage, string responseContent, object response) : base(statuscode, statusDescription, responseuri, errorMessage, responseContent, response)
        {
        }
    }

    public class MethodNotAllowedException : RequestException
    {
        public MethodNotAllowedException(int statuscode, string statusDescription, string responseuri, string errorMessage, string responseContent, object response) : base(statuscode, statusDescription, responseuri, errorMessage, responseContent, response)
        {
        }
    }

    internal class NotAcceptableException : RequestException
    {
        public NotAcceptableException(int statusCode, string statusDescription, string responseUri, string errorMessage, string responseContent, object response) : base(statusCode, statusDescription, responseUri, errorMessage, responseContent, response)
        {
        }
    }

    internal class RequestTimeoutException : RequestException
    {
        public RequestTimeoutException(int statusCode, string statusDescription, string responseUri, string errorMessage, string responseContent, object response) : base(statusCode, statusDescription, responseUri, errorMessage, responseContent, response)
        {
        }
    }

    internal class ConflictException : RequestException
    {
        public ConflictException(int statusCode, string statusDescription, string responseUri, string errorMessage, string responseContent, object response) : base(statusCode, statusDescription, responseUri, errorMessage, responseContent, response)
        {
        }
    }

    public class InternalServerErrorException : RequestException
    {
        public InternalServerErrorException(int statusCode, string statusDescription, string responseUri, string errorMessage, string responseContent, object response) : base(statusCode, statusDescription, responseUri, errorMessage, responseContent, response)
        {
        }
    }

    public class NotImplementedException : RequestException
    {
        public NotImplementedException(int statusCode, string statusDescription, string responseUri, string errorMessage, string responseContent, object response) : base(statusCode, statusDescription, responseUri, errorMessage, responseContent, response)
        {
        }
    }

    public class BadGateWayException : RequestException
    {
        public BadGateWayException(int statusCode, string statusDescription, string responseUri, string errorMessage, string responseContent, object response) : base(statusCode, statusDescription, responseUri, errorMessage, responseContent, response)
        {
        }
    }
}

using System;

namespace ApiLibs
{
    public class RequestException<T> : InvalidOperationException where T : RequestResponse
    {
        public T Response { get; set; }

    }

    public class RequestException : RequestException<RequestResponse>
    {
        public override string Message => $"Got {Response.StatusCode}:{Response.StatusDescription} while trying to access \"{Response.ResponseUri}\". {Response.ErrorMessage}";

        public RequestException(RequestResponse response) : base()
        {
            Response = response ?? throw new ArgumentNullException(nameof(response));
        }

        public static RequestException ConvertToException(RequestResponse response)
        {
            switch ((int)response.StatusCode)
            {
                case 204:
                    return new NoContentRequestException(response);

                case 400:
                    return new BadRequestException(response);
                case 401:
                    return new UnAuthorizedException(response);
                case 403:
                    return new ForbiddenException(response);
                case 404:
                    return new PageNotFoundException(response);
                case 405:
                    return new MethodNotAllowedException(response);
                case 406:
                    return new NotAcceptableException(response);
                case 408:
                    return new RequestTimeoutException(response);
                case 409:
                    return new ConflictException(response);

                case 500:
                    return new InternalServerErrorException(response);
                case 501:
                    return new NotImplementedException(response);
                case 502:
                    return new BadGateWayException(response);

                default:
                    return new RequestException(response);
            }
        }
    }

    public class NoInternetException : InvalidOperationException
    {
        public NoInternetException(Exception inner) : base(inner.Message, inner) { }
    }

    public class NoContentRequestException : RequestException
    {
        public NoContentRequestException(RequestResponse response) : base(response)
        {
        }
    }

    public class BadRequestException : RequestException
    {
        public BadRequestException(RequestResponse response) : base(response)
        {
        }
    }

    public class ForbiddenException : RequestException
    {
        public ForbiddenException(RequestResponse response) : base(response)
        {
        }
    }

    public class UnAuthorizedException : RequestException
    {
        public UnAuthorizedException(RequestResponse response) : base(response)
        {
        }
    }


    public class PageNotFoundException : RequestException
    {
        public PageNotFoundException(RequestResponse response) : base(response)
        {
        }
    }

    public class MethodNotAllowedException : RequestException
    {
        public MethodNotAllowedException(RequestResponse response) : base(response)
        {
        }
    }

    public class NotAcceptableException : RequestException
    {
        public NotAcceptableException(RequestResponse response) : base(response)
        {
        }
    }

    public class RequestTimeoutException : RequestException
    {
        public RequestTimeoutException(RequestResponse response) : base(response)
        {
        }
    }

    public class ConflictException : RequestException
    {
        public ConflictException(RequestResponse response) : base(response)
        {
        }
    }

    public class InternalServerErrorException : RequestException
    {
        public InternalServerErrorException(RequestResponse response) : base(response)
        {
        }
    }

    public class NotImplementedException : RequestException
    {
        public NotImplementedException(RequestResponse response) : base(response)
        {
        }
    }

    public class BadGateWayException : RequestException
    {
        public BadGateWayException(RequestResponse response) : base(response)
        {
        }
    }
}
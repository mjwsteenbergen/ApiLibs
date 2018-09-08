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
        public override string Message => $"Got {(int)Response.StatusCode}:{Response.StatusDescription} while trying to access \"{Response.ResponseUri}\". {Response.ErrorMessage} \n {Response.Content}";
        public IRestResponse Response { get; set; }

        public RequestException(IRestResponse response) : base()
        {
            Response = response;
        }
    }

    public class NoInternetException : InvalidOperationException
    {
        public NoInternetException(Exception inner) : base(inner.Message, inner) { }
    }

    public class PageNotFoundException : RequestException
    {
        public PageNotFoundException(IRestResponse response) : base(response)
        {
        }
    }

    public class UnAuthorizedException : RequestException
    {
        public UnAuthorizedException(IRestResponse response) : base(response)
        {
        }
    }

    public class BadRequestException : RequestException
    {
        public BadRequestException(IRestResponse response) : base(response)
        {
        }
    }
}

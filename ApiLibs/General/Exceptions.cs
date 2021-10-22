using System;

namespace ApiLibs
{
    public class RequestException : InvalidOperationException
    {
        public override string Message => $"Got {Response.StatusCode}:{Response.StatusDescription} while trying to access \"{Response.ResponseUri}\". {Response.ErrorMessage}";
        public RequestResponse Response { get; set; }

        public RequestException(RequestResponse response) : base()
        {
            Response = response ?? throw new ArgumentNullException(nameof(response));
        }
    }

    public class NoInternetException : InvalidOperationException
    {
        public NoInternetException(Exception inner) : base(inner.Message, inner) { }
    }
}

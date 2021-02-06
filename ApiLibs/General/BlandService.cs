using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ApiLibs.General
{
    public class BlandService : Service
    {
        public BlandService() : base("https://restsharp.dev/", false)
        {
            
        }

        public Task<T> Request<T>(string url, Call call = Call.GET, List<Param> parameters = null, List<Param> headers = null, object content = null, HttpStatusCode statusCode = HttpStatusCode.OK) => MakeRequest<T>(url, call, parameters, headers, content, statusCode);

        protected internal override Task<string> HandleRequest(string url, Call call = Call.GET, List<Param> parameters = null, List<Param> headers = null, object content = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            Uri uri = new Uri(url);
            Client.BaseUrl = new Uri(uri.OriginalString.Replace(uri.PathAndQuery, ""));
            return base.HandleRequest(uri.PathAndQuery, call, parameters, headers, content, statusCode);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ApiLibs
{
    public class BlandService : RestSharpService
    {
        public BlandService() : base("https://restsharp.dev/")
        {
            RequestMiddleware.Add((req) =>
            {
                Uri uri = new(req.EndPoint);
                (Implementation as RestSharpImplementation).Client = new RestSharp.RestClient(new Uri(uri.OriginalString.Replace(uri.PathAndQuery, "")));
                req.EndPoint = uri.PathAndQuery;
                return Task.FromResult(req);
            });
        }

        public Task<T> Request<T>(string url, Call call = Call.GET, List<Param> parameters = null, List<Param> headers = null, object content = null, HttpStatusCode statusCode = HttpStatusCode.OK) => MakeRequest<T>(url, call, parameters, headers, content, statusCode);
    }
}
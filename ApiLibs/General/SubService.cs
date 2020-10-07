using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace ApiLibs.General
{
    public class SubService<T> where T : Service
    {
        private readonly string partialEndpoint;

        protected T Service { get; }
        public SubService(T service, string partialEndpoint = "")
        {
            Service = service;
            this.partialEndpoint = partialEndpoint;
        }

        protected virtual async Task<S> MakeRequest<S>(string url, Call m = Call.GET, List<Param> parameters = null, List<Param> header = null, object content = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return await Service.MakeRequest<S>(partialEndpoint + url, m, parameters, header, content, statusCode);
        }

        protected virtual async Task<string> HandleRequest(string url, Call m = Call.GET, List<Param> parameters = null, List<Param> header = null, object content = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return await Service.HandleRequest(partialEndpoint + url, m, parameters, header, content, statusCode);
        }
    }
}

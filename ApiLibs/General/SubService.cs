using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace ApiLibs.General
{
    public class SubService
    {
        internal Service Service { get; }
        public SubService(Service service)
        {
            Service = service;
        }

        protected virtual async Task<T> MakeRequest<T>(string url, Call m = Call.GET, List<Param> parameters = null, List<Param> header = null, object content = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return await Service.MakeRequest<T>(url, m, parameters, header, content, statusCode);
        }

        protected virtual async Task<string> HandleRequest(string url, Call m = Call.GET, List<Param> parameters = null, List<Param> header = null, object content = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return await Service.HandleRequest(url, m, parameters, header, content, statusCode);
        }
    }

    public class SubService<T> where T : Service
    {
        protected T Service { get; }
        public SubService(T service)
        {
            Service = service;
        }

        protected virtual async Task<S> MakeRequest<S>(string url, Call m = Call.GET, List<Param> parameters = null, List<Param> header = null, object content = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return await Service.MakeRequest<S>(url, m, parameters, header, content, statusCode);
        }

        protected virtual async Task<string> HandleRequest(string url, Call m = Call.GET, List<Param> parameters = null, List<Param> header = null, object content = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return await Service.HandleRequest(url, m, parameters, header, content, statusCode);
        }
    }
}

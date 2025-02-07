using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ApiLibs.General;
using Newtonsoft.Json;
using OneOf;
using RestSharp;
using RestSharp.Authenticators;

namespace ApiLibs
{
    public abstract class RestSharpService : Service
    {
        // protected RestClient Client;

        protected RestSharpService(string hostUrl) : base(new RestSharpImplementation())
        {
            (Implementation as RestSharpImplementation).Client = new RestClient(hostUrl);
        }

        protected RestSharpService(RestClientOptions options) : base(new RestSharpImplementation())
        {
            (Implementation as RestSharpImplementation).Client = new RestClient(options);
        }
    }

    public abstract class Service
    {
        protected internal ICallImplementation Implementation { get; }
        private int? maxRetries;

        public virtual int? MaxRetries { get => maxRetries ?? 3; set => maxRetries = value; }

        protected readonly List<Func<Request, Task<Request>>> RequestMiddleware = new();
        protected readonly List<Func<RequestResponse, Task<RequestResponse>>> RequestResponseMiddleware = new();

        public Service(ICallImplementation implementation)
        {
            Implementation = implementation;

            RequestMiddleware.Add((req) =>
            {
                req.Headers.RemoveAll(i => standardHeaders.Any(j => i.Name == j.Name));
                req.Headers.AddRange(standardHeaders);

                req.Parameters.RemoveAll(i => standardParameters.Any(j => i.Name == j.Name));
                req.Parameters.AddRange(standardParameters);

                return Task.FromResult(req);
            });
        }

        private readonly List<Param> standardHeaders = new();
        protected void AddStandardHeader(string name, string content) => AddStandardHeader(new Param(name, content));
        protected void AddStandardHeader(Param p)
        {
            RemoveStandardHeader(p.Name);
            standardHeaders.Add(p);
        }
        protected void RemoveStandardHeader(string name)
        {
            standardHeaders.RemoveAll(p => p.Name == name);
        }

        private readonly List<Param> standardParameters = new();
        protected void AddStandardParameter(string name, string content) => AddStandardParameter(new Param(name, content));
        protected void AddStandardParameter(Param p)
        {
            RemoveStandardParameter(p.Name);
            standardParameters.Add(p);
        }
        protected void RemoveStandardParameter(string name)
        {
            standardParameters.RemoveAll(p => p.Name == name);
        }

        internal R Convert<R>(string text)
        {
            if (typeof(R) == typeof(string))
            {
                return (R)(object)text;
            }
            R returnObj;
            returnObj = JsonConvert.DeserializeObject<R>(text);
            if (returnObj is ObjectSearcher objectSearcher)
            {
                //Enable better OOP
                objectSearcher.Search(this);
            }
            return returnObj;
        }

        protected internal virtual async Task<RequestResponse> HandleRequest(Request request, IEnumerable<Func<Request, Task<Request>>> requestMiddleware = null, IEnumerable<Func<RequestResponse, Task<RequestResponse>>> requestResponseMiddleware = null)
        {
            if (request.Retried > (request.MaxRetries ?? MaxRetries))
            {
                throw new TooManyRetriesException();
            }

            requestMiddleware ??= new List<Func<Request, Task<Request>>>();
            foreach (var func in RequestMiddleware.Concat(requestMiddleware))
            {
                request = await func(request);
            }

            var resp = await Implementation.ExecuteRequest(this, request);

            requestResponseMiddleware ??= new List<Func<RequestResponse, Task<RequestResponse>>>();
            foreach (var func in RequestResponseMiddleware.Concat(requestResponseMiddleware))
            {
                resp = await func(resp);
            }

            return resp;
        }

        protected internal async Task<T> MakeRequest<T>(Request<T> request)
        {
            var response = await HandleRequest(request);
            response = await request.RequestHandler(response);
            return request.ParseHandler(response);
        }

        protected internal async Task MakeRequest(Request request)
        {
            await request.RequestHandler(await HandleRequest(request));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">The class we expect the response text to be</typeparam>
        /// <param name="endPoint">endPoint to call</param>
        /// <param name="method">method to use when calling the endpoint</param>
        /// <param name="parameters">the list of parameters in the url to add.</param>
        /// <param name="headers">the headers to add to the request</param>
        /// <param name="content">The content of the request-body</param>
        /// <param name="statusCode">the headers to add to the request</param>
        /// <returns></returns>
        protected internal async Task<T> MakeRequest<T>(string endPoint, Call method = Call.GET, List<Param> parameters = null, List<Param> headers = null, object content = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return await MakeRequest(new Request<T>(endPoint)
            {
                Content = content,
                Headers = headers ?? new List<Param>(),
                Parameters = parameters ?? new List<Param>(),
                Method = method,
                ExpectedStatusCode = statusCode
            });
        }

        protected internal async Task MakeRequest(string endPoint, Call method = Call.GET, List<Param> parameters = null, List<Param> headers = null, object content = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            await MakeRequest(new Request(endPoint)
            {
                Content = content,
                Headers = headers ?? new List<Param>(),
                Parameters = parameters ?? new List<Param>(),
                Method = method,
                ExpectedStatusCode = statusCode
            });
        }
    }

    public class TooManyRetriesException : Exception { }

    public class Request
    {
        public Request(string endPoint)
        {
            EndPoint = endPoint;
            Retried = 0;
            RequestHandler = (resp) =>
            {
                var matchesStatusCode = ExpectedStatusCode
                    .Match(i => new HttpStatusCode[] { i }, i => i)
                    .Any(i => i == resp.StatusCode);
                if (!matchesStatusCode)
                {
                    throw resp.ToException();
                }

                return Task.FromResult(resp);
            };

            Parameters = new();
            Headers = new();
            ExpectedStatusCode = HttpStatusCode.OK;
        }

        public string EndPoint { get; set; }
        public Call Method { get; set; } = Call.GET;
        public List<Param> Parameters { get; set; }
        public List<Param> Headers { get; set; }
        public TimeSpan? Timeout { get; set; }
        public object Content { get; set; }
        public OneOf<HttpStatusCode, IEnumerable<HttpStatusCode>> ExpectedStatusCode { get; set; }

        public int Retried { get; set; }
        public Func<RequestResponse, Task<RequestResponse>> RequestHandler { get; set; }
        public int? MaxRetries { get; set; }
    }

    public class Request<T> : Request
    {
        public Request(string endPoint) : base(endPoint)
        {
            ParseHandler = (resp) => resp.Convert<T>();
        }

        public Func<RequestResponse, T> ParseHandler { get; set; }
    }

    public enum Call
    {
        POST,
        GET,
        PATCH,
        DELETE,
        PUT
    }
}



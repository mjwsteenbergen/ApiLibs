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
        protected RestClient Client;

        protected RestSharpService(string hostUrl) : base(new RestSharpImplementation())
        {
            Client = new RestClient { BaseUrl = new Uri(hostUrl) };
            (Implementation as RestSharpImplementation).Client = Client;
        }

        protected void ConnectBasic(string username, string secret)
        {
            Client.Authenticator = new HttpBasicAuthenticator(username, secret);
        }

        protected void SetBaseUrl(string baseurl)
        {
            Client.BaseUrl = new Uri(baseurl);
        }
    }

    public abstract class Service
    {
        internal ICallImplementation Implementation { get; }

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
            if (returnObj is ObjectSearcher)
            {
                //Enable better OOP
                (returnObj as ObjectSearcher).Search(this);
            }
            return returnObj;
        }

        protected internal virtual async Task<RequestResponse> HandleRequest(Request request, IEnumerable<Func<Request, Task<Request>>> requestMiddleware = null, IEnumerable<Func<RequestResponse, Task<RequestResponse>>> requestResponseMiddleware = null)
        {
            if(request.Retries > 3)
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
            var resp = await HandleRequest(new Request(endPoint)
            {
                Content = content,
                Headers = headers ?? new List<Param>(),
                Parameters = parameters ?? new List<Param>(),
                Method = method,
            });

            if (resp.StatusCode == statusCode)
            {
                return Convert<T>(resp.Content);
            }
            else
            {
                throw RequestException.ConvertToException(resp);
            }
        }

        protected internal async Task<E0> MakeRequestExpert<E0>(string endPoint, Call method = Call.GET, List<Param> parameters = null, List<Param> headers = null, object content = null)
            where E0 : GenericBaseRequestResponse, new()
        {
            var resp = await HandleRequest(new Request(endPoint)
            {
                Content = content,
                Headers = headers ?? new List<Param>(),
                Parameters = parameters ?? new List<Param>(),
                Method = method,
            });

            {
                var e0 = new E0
                {
                    Response = resp
                };

                if (e0.StatusCode == resp.StatusCode)
                {
                    return e0;
                }
            }

            throw RequestException.ConvertToException(resp);
        }



        protected internal async Task<E0> MakeRequest<E0, E1>(string endPoint, Call method = Call.GET, List<Param> parameters = null, List<Param> headers = null, object content = null, Func<E1, RequestResponse, Task<RequestResponse>> funcE1 = null)
            where E0 : GenericBaseRequestResponse, new()
            where E1 : GenericBaseRequestResponse, new()
        {
            var resp = await HandleRequest(new Request(endPoint)
            {
                Content = content,
                Headers = headers ?? new List<Param>(),
                Parameters = parameters ?? new List<Param>(),
                Method = method,
            }, requestResponseMiddleware: new List<Func<RequestResponse, Task<RequestResponse>>>() {
                (resp) =>  {
                    {
                        var e1 = new E1
                        {
                            Response = resp
                        };

                        if (e1.StatusCode == resp.StatusCode)
                        {
                            return funcE1?.Invoke(e1, resp);
                        }
                    }

                    return Task.FromResult(resp);
                }
            });

            var e0 = new E0
            {
                Response = resp
            };

            if (e0.StatusCode == resp.StatusCode)
            {
                return e0;
            }
            else
            {
                throw RequestException.ConvertToException(resp);
            }
        }
    }

    public class TooManyRetriesException : Exception {}

    public class Request
    {
        public Request(string endPoint)
        {
            EndPoint = endPoint;
            Retries = 0;
        }

        public string EndPoint { get; set; }
        public Call Method { get; set; } = Call.GET;
        public List<Param> Parameters { get; set; }
        public List<Param> Headers { get; set; }
        public object Content { get; set; }

        public int Retries { get; set; }
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



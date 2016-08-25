using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;

namespace ApiLibs
{
    public abstract class Service
    {
        internal RestClient Client;
        private readonly List<Param> _standardParameter = new List<Param>();
        private readonly List<Param> _standardHeader = new List<Param>();

        public void SetUp(string hostUrl)
        {
            Client = new RestClient {BaseUrl = new Uri(hostUrl)};
        }

        internal void AddStandardParameter(Param p)
        {
            _standardParameter.Add(p);
        }

        internal void AddStandardHeader(Param p)
        {
            _standardHeader.Add(p);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("SonarLint", "S2228:Console logging should not be used", Justification = "I can")]
        internal void UpdateParameterIfExists(Param p)
        {
            foreach (Param para in _standardParameter)
            {
                if (para.Name == p.Name)
                {
                    Console.WriteLine(para.Name + " was: " + para.Value + " is: " + p.Value);
                    para.Value = p.Value;
                    
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("SonarLint", "S2228:Console logging should not be used", Justification = "I can")]
        internal void UpdateHeaderIfExists(Param p)
        {
            foreach (Param para in _standardHeader)
            {
                if (para.Name == p.Name)
                {
                    Console.WriteLine(para.Name + " was: " + para.Value + " is: " + p.Value);
                    para.Value = p.Value;

                }
            }
        }

        internal void ConnectOAuth(string username, string secret)
        {
            Client.Authenticator = new HttpBasicAuthenticator(username, secret);
        }
        

        internal async Task<T> MakeRequest<T>(string url, Call m = Call.GET, List<Param> parameters = null, List<Param> header = null, object content = null)
        {
            var request = new RestRequest(url, Convert(m));


            var rParameters = parameters ?? new List<Param>();

            var rHeaders = header ?? new List<Param>();


            if (content != null)
            {
                request.AddParameter("application/json", JsonConvert.SerializeObject(content), ParameterType.RequestBody);
                request.AddHeader("content-type", "application/json");
            }

            IRestResponse response = await HandleRequest(request, rParameters, rHeaders);

            var typeOfT = typeof(T);

            if (typeOfT == typeof(IRestResponse))
            {
                return (T)response;
            }

            return Convert<T>(response);
        }

        internal async Task<IRestResponse> HandleRequest(string url, Call call = Call.GET, List<Param> parameters = null, List<Param> headers = null)
        {
            RestRequest request = new RestRequest(url, Convert(call));
            return await HandleRequest(request, parameters, headers);
        }

        internal async Task<IRestResponse> HandleRequest(IRestRequest request, List <Param> parameters = null, List<Param> headers = null)
        {
            if (headers != null)
            {
                foreach (Param p in headers)
                {
                    request.AddHeader(p.Name, p.Value);
                }
            }

            if (parameters != null)
            {
                foreach (Param para in parameters)
                {
                    request.AddParameter(para.Name, para.Value);
                }
            }

            foreach (Param para in _standardParameter)
            {
                request.AddParameter(para.Name, para.Value);
            }

            foreach (Param para in _standardHeader)
            {
                request.AddHeader(para.Name, para.Value);
            }

            return await ExcecuteRequest(request);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("SonarLint", "S2228:Console logging should not be used", Justification = "I can")]
        internal async Task<IRestResponse> ExcecuteRequest(IRestRequest request)
        {
            Debug.Assert(Client != null, "Client != null");
            IRestResponse resp = await Client.ExecuteTaskAsync(request);

            if (resp.StatusCode.ToString() != "OK" && resp.StatusCode.ToString() != "Created" && resp.StatusCode.ToString() != "ResetContent")
            {
                foreach (Parameter p in resp.Headers)
                {
                    Console.WriteLine(p.ToString());
                }
                if (resp.ErrorException != null)
                {
                    if (resp.ErrorException is System.Net.WebException)
                    {
                        throw new NoInternetException(resp.ErrorException);
                    }

                    throw resp.ErrorException;
                }
                throw new RequestException(resp.ResponseUri.ToString(), resp.StatusCode, resp.Content);



            }
            return resp;
        }

        internal T Convert<T>(IRestResponse resp)
        {
            return Convert<T>(resp.Content);
        }

        internal T Convert<T>(string text)
        {
            return JsonConvert.DeserializeObject<T>(text);
        }

        private Method Convert(Call m)
        {
            switch (m)
            {
                case Call.POST:
                    return Method.POST;
                case Call.PATCH:
                    return Method.PATCH;
                case Call.DELETE:
                    return Method.DELETE;
                case Call.PUT:
                    return Method.PUT;
                default:
                    return Method.GET;
            }
        }

        internal void SetBaseUrl(string baseurl)
        {
            Client.BaseUrl = new Uri(baseurl);
        }

        internal void Print(IRestResponse resp)
        {
            Console.WriteLine(resp.Content);
        }

        
    }
}

public class NoInternetException : InvalidOperationException
{
    public NoInternetException(Exception inner) : base(inner.Message, inner)
    {
        
    }
}

public class RequestException : InvalidOperationException
{
    public readonly string ResponseUri;
    public readonly HttpStatusCode StatusCode;
    public readonly string Content;

    public RequestException(string responseUri, HttpStatusCode statusCode, string content) : base("A problem occured while trying to access " + responseUri + ". Statuscode: " + statusCode + "\n" + content)
    {
        ResponseUri = responseUri;
        StatusCode = statusCode;
        Content = content;
    }
}

enum Call
{
    POST,
    GET,
    PATCH,
    DELETE,
    PUT
}

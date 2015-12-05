using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;

namespace ApiLibs
{
    public abstract class Service
    {
        //TODO convert POST to argument

        internal RestClient Client;
        private readonly List<Param> _standardParameter = new List<Param>();
        private readonly List<Param> _standardHeader = new List<Param>();

        public void SetUp(string hostUrl)
        {
            Client = new RestClient {BaseUrl = new Uri(hostUrl)};
            Passwords.ReadPasswords();
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



        internal async Task<IRestResponse> MakeRequest(string url, List<Param> parameters, object jsonBody)
        {
            RestRequest request = new RestRequest(url, Method.GET);
            request.AddJsonBody(jsonBody);
            return await MakeRequest(request, parameters);
        }

        internal async Task<T> MakeRequest<T>(string url, List<Param> parameters)
        {
            RestRequest request = new RestRequest(url, Method.GET);
            return Convert<T>(await MakeRequest(request, parameters));
        }

        internal async Task<IRestResponse> MakeRequest(string url, List<Param> parameters)
        {
            RestRequest request = new RestRequest(url, Method.GET);
            return await MakeRequest(request, parameters);
        }

        internal async Task<T> MakeRequestPost<T>(string url, List<Param> parameters, object content)
        {
            RestRequest request = new RestRequest(url, Method.POST);
            request.AddParameter("application/json", JsonConvert.SerializeObject(content), ParameterType.RequestBody);
            request.AddHeader("content-type", "application/json");
            return await MakeRequest<T>(request, parameters, new List<Param>());
        }

        internal async Task<IRestResponse> MakeRequestPost(string url, List<Param> parameters)
        {
            RestRequest request = new RestRequest(url, Method.POST);
            return await MakeRequest(request, parameters);
        }

        internal async Task<T> MakeRequestPost<T>(string url, List<Param> parameters)
        {
            return await MakeRequest<T>(new RestRequest(url, Method.POST), parameters, new List<Param>());
            
        }

        internal async Task<T> MakeRequestPatch<T>(string url, object obj)
        {
            var request = new RestRequest(url, Method.PATCH);
            request.AddParameter("application/json", JsonConvert.SerializeObject(obj), ParameterType.RequestBody);
            request.AddHeader("content-type", "application/json");
            return await MakeRequest<T>(request, new List<Param>(), new List<Param>());
        }

        internal async Task<T> MakeRequest<T>(Method method, string url, object obj)
        {
            var request = new RestRequest(url, method);
            request.AddParameter("application/json", JsonConvert.SerializeObject(obj), ParameterType.RequestBody);
            request.AddHeader("content-type", "application/json");
            return await MakeRequest<T>(request, new List<Param>(), new List<Param>());
        }

        internal async Task MakeRequest(Method m, string url, List<Param> parameters)
        {
            await MakeRequest(new RestRequest(url, m), parameters, new List<Param>());
        }

        internal async Task<T> MakeRequest<T>(RestRequest request, List<Param> parameters, List<Param> headers)
        {
            return Convert<T>(await MakeRequest(request, parameters, headers));
        }

        internal async Task<IRestResponse> MakeRequest(RestRequest request, List<Param> parameters, List<Param> headers)
        {
            foreach (Param p in headers)
            {
                request.AddHeader(p.Name, p.Value);
            }
            return await MakeRequest(request, parameters);
        }

        internal async Task<IRestResponse> MakeRequest(IRestRequest request, List<Param> parameters)
        {
            foreach (Param para in parameters)
            {
                request.AddParameter(para.Name, para.Value);
            }

            foreach (Param para in _standardParameter)
            {
                request.AddParameter(para.Name, para.Value);
            }

            foreach (Param para in _standardHeader)
            {
                request.AddHeader(para.Name, para.Value);
            }

            return await MakeRequest(request);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("SonarLint", "S2228:Console logging should not be used", Justification = "I can")]
        internal async Task<IRestResponse> MakeRequest(IRestRequest request)
        {
            Debug.Assert(Client != null, "Client != null");
            IRestResponse resp = Client.Execute(request);

            if (resp.StatusCode.ToString() != "OK" && resp.StatusCode.ToString() != "Created")
            {
                foreach (Parameter p in resp.Headers)
                {
                    Console.WriteLine(p.ToString());
                }
                throw new Exception("A problem occured while trying to access " + resp.ResponseUri + ". Statuscode: " + resp.StatusCode + "\n" + resp.Content);

            }
            return resp;
        }

        internal T Convert<T>(IRestResponse resp)
        {
            return JsonConvert.DeserializeObject<T>(resp.Content);
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

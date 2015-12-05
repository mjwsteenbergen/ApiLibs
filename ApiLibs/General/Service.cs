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

        internal RestClient client;
        private List<Param> standardParameter = new List<Param>();
        private List<Param> standardHeader = new List<Param>();

        public void SetUp(string hostUrl)
        {
            client = new RestClient();
            client.BaseUrl = new Uri(hostUrl);
            Passwords.readPasswords();
        }

        internal void AddStandardParameter(Param p)
        {
            standardParameter.Add(p);
        }

        internal void AddStandardHeader(Param p)
        {
            standardHeader.Add(p);
        }

        internal void UpdateParameterIfExists(Param p)
        {
            foreach (Param para in standardParameter)
            {
                if (para.name == p.name)
                {
                    Console.WriteLine(para.name + " was: " + para.value + " is: " + p.value);
                    para.value = p.value;
                    
                }
            }
        }

        internal void UpdateHeaderIfExists(Param p)
        {
            foreach (Param para in standardHeader)
            {
                if (para.name == p.name)
                {
                    Console.WriteLine(para.name + " was: " + para.value + " is: " + p.value);
                    para.value = p.value;

                }
            }
        }

        internal void ConnectOAuth(string username, string secret)
        {
            client.Authenticator = new HttpBasicAuthenticator(username, secret);
        }



        internal async Task<IRestResponse> MakeRequest(string url, List<Param> parameters, object JSonBody)
        {
            RestRequest request = new RestRequest(url, Method.GET);
            request.AddJsonBody(JSonBody);
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
            return await MakeRequest(request, parameters);
        }

        internal async Task<IRestResponse> MakeRequest(IRestRequest request, List<Param> parameters)
        {
//            request.RequestFormat = DataFormat.Json;
//            request.JsonSerializer.ContentType = "application/json; charset=utf-8";
//
            foreach (Param para in parameters)
            {
                request.AddParameter(para.name, para.value);
            }

            foreach (Param para in standardParameter)
            {
                request.AddParameter(para.name, para.value);
            }
//
            foreach (Param para in standardHeader)
            {
                request.AddHeader(para.name, para.value);
            }


            return await MakeRequest(request);
        }

        internal async Task<IRestResponse> MakeRequest(IRestRequest request)
        {
            Debug.Assert(client != null, "client != null");
            IRestResponse resp = client.Execute(request);

            if (resp.StatusCode.ToString() != "OK" && resp.StatusCode.ToString() != "Created")
            {
                foreach (Parameter p in resp.Headers)
                {
                    Console.WriteLine(p.ToString());
                }
                throw new Exception("A problem occured while trying to access " + resp.ResponseUri + ". Statuscode: " + resp.StatusCode.ToString() + "\n" + resp.Content);

            }
            return resp;
        }

        internal T Convert<T>(IRestResponse resp)
        {
            return JsonConvert.DeserializeObject<T>(resp.Content);
        }

        

        internal void Dispose()
        {
            client = null;
        }

        internal void setBaseUrl(string baseurl)
        {
            client.BaseUrl = new Uri(baseurl);
        }

        internal void Print(IRestResponse resp)
        {
            Console.WriteLine(resp.Content);
        }
    }
}

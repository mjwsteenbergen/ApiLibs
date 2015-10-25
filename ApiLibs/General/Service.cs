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

        private RestClient client;
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

        internal void ConnectOAuth(string username, string secret)
        {
            client.Authenticator = new HttpBasicAuthenticator(username, secret);
        }



        internal async Task<IRestResponse> MakeRequest(string url, List<Param> parameters, object JSonBody)
        {
            RestRequest request = new RestRequest(url, Method.GET);
            request.AddJsonBody(JSonBody);
            return await addParametersAndMakeCall(request, parameters);
        }

        internal async Task<T> MakeRequest<T>(string url, List<Param> parameters)
        {
            RestRequest request = new RestRequest(url, Method.GET);
            return Convert<T>(await addParametersAndMakeCall(request, parameters));
        }

        internal async Task<IRestResponse> MakeRequest(string url, List<Param> parameters)
        {
            RestRequest request = new RestRequest(url, Method.GET);
            return await addParametersAndMakeCall(request, parameters);
        }

        internal async Task<IRestResponse> MakeRequestPost(string url, List<Param> parameters)
        {
            RestRequest request = new RestRequest(url, Method.POST);
            return await addParametersAndMakeCall(request, parameters);
        }



        private async Task<IRestResponse> addParametersAndMakeCall(IRestRequest request, List<Param> parameters)
        {
            request.RequestFormat = DataFormat.Json;
            request.JsonSerializer.ContentType = "application/json; charset=utf-8";

            foreach (Param para in parameters)
            {
                request.AddParameter(para.name, para.value);
            }

            foreach (Param para in standardParameter)
            {
                request.AddParameter(para.name, para.value);
            }

            foreach (Param para in standardHeader)
            {
                request.AddHeader(para.name, para.value);
            }


            return await performCall(request);
        }

        private async Task<IRestResponse> performCall(IRestRequest request)
        {
            Debug.Assert(client != null, "client != null");
            IRestResponse resp = await client.ExecuteTaskAsync(request,
            new CancellationToken());

            if (resp.StatusCode.ToString() != "OK")
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
    }
}

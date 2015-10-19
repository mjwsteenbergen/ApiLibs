using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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

        internal async Task<IRestResponse> MakeRequest(String url, List<Param> parameters)
        {

            RestRequest request = new RestRequest(url, Method.GET);
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

           

            Debug.Assert(client != null, "client != null");
            IRestResponse resp = await client.ExecuteTaskAsync(request,
            new CancellationToken());

            if (resp.StatusCode.ToString() != "OK")
            {
                foreach (Parameter p in resp.Headers)
                {
                    Console.WriteLine(p.ToString());
                }
                throw new Exception("A problem occured while trying to access " + url + ". Statuscode: " + resp.StatusCode.ToString() + "\n" + resp.Content);

            }

            return resp;
            
            

            
        }

        internal async Task<IRestResponse> MakeRequestPost(string authGithub, List<Param> head)
        {
            RestRequest request = new RestRequest(authGithub, Method.POST);

            foreach (Param para in head)
            {
                request.AddParameter(para.name, para.value);
            }

            foreach (Param para in standardParameter)
            {
                request.AddParameter(para.name, para.value);
            }

            try
            {
                Debug.Assert(client != null, "client != null");
                return await client.ExecuteTaskAsync(request,
                    new CancellationToken());
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in:" + this.GetType() + " " + e.Message);
                return null;
            }
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

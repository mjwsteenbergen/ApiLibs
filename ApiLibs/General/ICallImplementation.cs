using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using ApiLibs.General;
using Newtonsoft.Json;
using RestSharp;

namespace ApiLibs
{
    public abstract class ICallImplementation
    {

        public abstract Task<RequestResponse> ExecuteRequest(Service service2, Request request);
    }

    public class RestSharpImplementation : ICallImplementation
    {
        public RestClient Client { get; internal set; }

        public override async Task<RequestResponse> ExecuteRequest(Service service, Request aRequest)
        {
            RestRequest request = new(aRequest.EndPoint, Convert(aRequest.Method));

            foreach (Param para in aRequest.Headers ?? new List<Param>())
            {
                if (para is OParam && para.Value == null)
                {
                    continue;
                }

                request.AddHeader(para.Name, para.Value);
            }

            foreach (Param para in aRequest.Parameters ?? new List<Param>())
            {
                if (para is OParam && para.Value == null)
                {
                    continue;
                }

                if (request.Method == Method.GET || request.Method == Method.POST)
                {

                    request.AddParameter(para.Name, para.Value);
                }
                else
                {
                    request.AddParameter(para.Name, para.Value, ParameterType.QueryString);
                }
            }


            if (aRequest.Content != null)
            {
                AddBody(request, aRequest.Content);
            }

            Debug.Assert(Client != null, "Client != null");
            IRestResponse resp = await Client.ExecuteAsync(request);

            if (resp.ErrorException != null)
            {
                if (resp.ErrorException is WebException)
                {

                    throw new NoInternetException(resp.ErrorException);
                }

                throw resp.ErrorException;
            }

            return new RequestResponse(resp.StatusCode, resp.StatusDescription, resp.ResponseUri.ToString(), resp.ErrorMessage, resp.Content, resp, aRequest, service);
        }

        private static void AddBody(IRestRequest request, object content)
        {
            switch (content)
            {
                case string text:
                    request.AddParameter("application/json", text, ParameterType.RequestBody);
                    request.AddHeader("Content-Type", "application/json");
                    break;
                case RequestContent rcontent:
                    request.AddParameter(rcontent.ContentType, rcontent.Content, ParameterType.RequestBody);
                    request.AddHeader("Content-Type", rcontent.ContentType);
                    break;
                default:
                    JsonSerializerSettings settings = new()
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    };
                    string jsonText = JsonConvert.SerializeObject(content, settings);
                    request.AddParameter("application/json", jsonText, ParameterType.RequestBody);
                    request.AddHeader("Content-Type", "application/json");
                    break;
            }
        }

        private Method Convert(Call m)
        {
            return m switch
            {
                Call.POST => Method.POST,
                Call.PATCH => Method.PATCH,
                Call.DELETE => Method.DELETE,
                Call.PUT => Method.PUT,
                _ => Method.GET,
            };
        }
    }
}
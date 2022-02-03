using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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

                if (request.Method == Method.Get || request.Method == Method.Post)
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
            RestResponse resp = await Client.ExecuteAsync(request);

            if (resp.ErrorException != null && resp.ErrorException is not HttpRequestException)
            {
                var rex = resp.ErrorException switch
                {
                    WebException webE when webE.Message == "No such host is known." => new NoInternetException(resp.ErrorException),
                    _ => resp.ErrorException,
                };
                throw rex;
            }

            return new RequestResponse(resp.StatusCode, resp.StatusDescription, resp.ResponseUri.ToString(), resp.ErrorMessage, resp.Content, resp, aRequest, service);
        }

        private static void AddBody(RestRequest request, object content)
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
                    request.AddHeader("Content-Type", "application/json");
                    request.AddBody(jsonText, "application/json");
                    break;
            }
        }

                    //     var client = new RestClient("https://api.pushcut.io/rGUKVq1jlGY0JIf5p_Odj/widgets/Widget?content=Status");
            //     var request = new RestRequest();
            //     request.AddHeader("Content-Type", "application/json");
            //     // request.AddJsonBody(new
            //     // {
            //     //     content = "Status",
            //     //     inputs = new { 
            //     //         input0 = "108",
            //     //         input1 = "20", 
            //     //         input2 = "8", 
            //     //         input3 = "82.70 %",
            //     //         input4 = "work" 
            //     //     }
            //     // });
            //     // request.AddJsonBody("{\"content\":\"Status\",\"inputs\":{\"input0\":\"108\",\"input1\":\"20\",\"input2\":\"8\",\"input3\":\"82.70%\",\"input4\":\"work\"}}");
            //     RestResponse response = await client.PostAsync(request);
            //     Console.WriteLine(response.Content);

        private Method Convert(Call m)
        {
            return m switch
            {
                Call.POST => Method.Post,
                Call.PATCH => Method.Patch,
                Call.DELETE => Method.Delete,
                Call.PUT => Method.Put,
                _ => Method.Get,
            };
        }
    }
}
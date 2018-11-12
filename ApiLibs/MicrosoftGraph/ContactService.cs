using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ApiLibs.General;
using RestSharp;

namespace ApiLibs.MicrosoftGraph
{
    public class ContactService : SubService
    {
        public ContactService(GraphService service) : base(service) { }

        internal override Task<T> MakeRequest<T>(string url, Call m = Call.GET, List<Param> parameters = null, List<Param> header = null, object content = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return base.MakeRequest<T>("v1.0/" + url, m, parameters, header, content, statusCode);
        }

        internal override Task<IRestResponse> HandleRequest(string url, Call m = Call.GET, List<Param> parameters = null, List<Param> header = null, object content = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return base.HandleRequest("v1.0/" + url, m, parameters, header, content, statusCode);
        }

        public async Task<List<Contact>> GetAllContacts()
        {
            return (await MakeRequest<ContactResult>($"/me/contacts?$top=100")).Value;

        }

        public async Task<List<Contact>> GetContacts(string name)
        {
            return (await MakeRequest<ContactResult>($"/me/contacts?$search=\"{name}\"")).Value;
        }

    }
}

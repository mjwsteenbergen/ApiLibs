using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ApiLibs.General;
using RestSharp;

namespace ApiLibs.MicrosoftGraph
{
    public class ContactService : GraphSubService
    {
        public ContactService(GraphService service) : base(service, "v1.0") { }

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

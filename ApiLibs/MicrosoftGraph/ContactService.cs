using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ApiLibs.General;

namespace ApiLibs.MicrosoftGraph
{
    public class ContactService : SubService
    {
        public ContactService(GraphService service) : base(service) { }

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

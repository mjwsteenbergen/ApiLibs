using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiLibs.MicrosoftGraph
{
    public class ContactService : GraphSubService
    {
        public ContactService(GraphService service) : base(service, "v1.0") { }

        public async IAsyncEnumerable<Contact> GetAllContacts()
        {
            int skip = 0;
            for (int i = 0; i < 30; i++)
            {
                var res = (await MakeRequest<ContactResult>($"/me/contacts?$top=100&$skip={skip}")).Value;

                if(res.Count == 0)
                {
                    break;
                }

                foreach (var item in res)
                {
                    yield return item;
                }
                skip+=100;
            }
        }

        public async Task<List<Contact>> GetContacts(string name)
        {
            return (await MakeRequest<ContactResult>($"/me/contacts?$search=\"{name}\"")).Value;
        }

    }
}

using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ApiLibs.General;
using RestSharp;

namespace ApiLibs.MicrosoftGraph
{
    public class PeopleService : GraphSubService
    {
        public PeopleService(GraphService service) : base(service, "v1.0")
        {
        }

        public async Task<List<Person>> GetAllContacts()
        {
            List<Person> people = new List<Person>();
            PersonResult result;
            int skip = 0;
            do
            {
                result = (await MakeRequest<PersonResult>($"/me/people?$top=100&$skip={skip}"));
                people.AddRange(result.Value);
                skip += 100;

            } while (result.OdataNextLink != null);
            
            return people;
        }

        public async Task<List<Person>> GetContacts(string name)
        {
            return (await MakeRequest<PersonResult>($"/me/people?$search=\"{name}\"")).Value;
        }
    }
}

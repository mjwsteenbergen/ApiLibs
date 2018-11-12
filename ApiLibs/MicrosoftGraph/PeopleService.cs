using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ApiLibs.General;
using RestSharp;

namespace ApiLibs.MicrosoftGraph
{
    public class PeopleService : SubService
    {
        public PeopleService(GraphService service) : base(service)
        {
        }

        internal override Task<T> MakeRequest<T>(string url, Call m = Call.GET, List<Param> parameters = null, List<Param> header = null, object content = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return base.MakeRequest<T>("v1.0/" + url, m, parameters, header, content, statusCode);
        }

        internal override Task<IRestResponse> HandleRequest(string url, Call m = Call.GET, List<Param> parameters = null, List<Param> header = null, object content = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return base.HandleRequest("v1.0/" + url, m, parameters, header, content, statusCode);
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

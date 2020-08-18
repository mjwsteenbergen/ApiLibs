using ApiLibs.General;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Martijn.Extensions.Linq;

namespace ApiLibs.Trakt
{
    public class SearchService : SubService
    {
        public SearchService(TraktService service) : base(service)
        {
        }

        public Task<List<WrappedMediaObject>> Search(List<string> types, string query) => MakeRequest<List<WrappedMediaObject>>($"/search/{types.Combine((i, j) => i + "," + j)}?query={query}");
        
        public Task<List<WrappedMediaObject>> Search(string type, string query) => Search(new List<string> { type}, query);
    }
}

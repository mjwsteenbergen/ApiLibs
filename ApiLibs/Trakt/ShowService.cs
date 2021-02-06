using ApiLibs.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiLibs.Trakt
{
    public class ShowService : SubService<TraktService>
    {
        public ShowService(TraktService service) : base(service)
        {
        }

        public Task<Show> GetShow(string id) => MakeRequest<Show>($"/shows/{id}?extended=full");
        public async Task<List<SeasonSmall>> GetSeasonsOfShow(ShowSmall show) => (await MakeRequest<List<SeasonSmall>>($"/shows/{show.Ids.Trakt}/seasons")).Select(i =>
        {
            i.Title = show.Title + $" [Season {i.Number}]";
            i.Show = show;
            return i;
        }).ToList();
    }
}

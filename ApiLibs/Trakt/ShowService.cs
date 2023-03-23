using ApiLibs.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        public Task<ShowSmall> GetShowSmall(string id) => MakeRequest<ShowSmall>($"/shows/{id}");
        public async Task<List<SeasonSmall>> GetSeasonsOfShowSmall(ShowSmall show) => (await MakeRequest<List<SeasonSmall>>($"/shows/{show.Ids.Trakt}/seasons")).Select(i =>
        {
            i.Title = show.Title + $" [Season {i.Number}]";
            i.Show = show;
            return i;
        }).ToList();

        public async Task<List<SeasonEpisodes>> GetSeasonsOfShowEpisodes(ShowSmall show) => (await MakeRequest<List<SeasonEpisodes>>($"/shows/{show.Ids.Trakt}/seasons?extended=episodes")).Select(i =>
        {
            i.Title = show.Title + $" [Season {i.Number}]";
            i.Show = show;
            return i;
        }).ToList();
        

        public Task<List<Episode>> GetEpisodes(string traktSlug, long season) => MakeRequest<List<Episode>>($"/shows/{traktSlug}/seasons/{season}");

        public Task<EpisodeExtended> LastEpisode(long? id) => MakeRequest(new Request<EpisodeExtended>($"/shows/{id}/last_episode?extended=full")
        {
            ExpectedStatusCode = new HttpStatusCode[] { HttpStatusCode.NoContent, HttpStatusCode.OK },
            ParseHandler = (resp) => 
                resp.StatusCode switch
                {
                    HttpStatusCode.NoContent => null,
                    HttpStatusCode.OK => Convert<EpisodeExtended>(resp.Content),
                    _ => throw resp.ToException()
                }
        });

        public Task<EpisodeExtended> GetEpisode(Ids episode) => MakeRequest<List<ExtendedWrappedMediaObject>>($"/search/trakt/{episode.Trakt}?id_type=episode&extended=full").ContinueWith(i => i.Result[0].Episode);

        public Task<List<Episode>> GetSeason(Ids season) => MakeRequest<List<Episode>>($"/search/trakt/{season.Trakt}?id_type=season");
    }
}

using ApiLibs.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ApiLibs.Trakt
{
    public class UserService : SubService<TraktService>
    {
        public UserService(TraktService service) : base(service)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">Possible values movies, shows, seasons or episodes</param>
        /// <param name="id"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public Task<List<WrappedMediaObject>> GetHistory(string type = null, string id = null, DateTime? start = null, DateTime? end = null)
        {
            string typeUrlPart = type switch
            {
                "movies" => "movies/",
                "shows" => "shows/",
                "seasons" => "seasons/",
                "episodes" => "episodes/",
                null => "",
                _ => throw new Exception("Invalid type " + type),
            };
            
            return MakeRequest<List<WrappedMediaObject>>("sync/history/" + typeUrlPart, Call.GET, new List<Param>
            {
                new OParam("id", id),
                new OParam("start_at", start?.ToString("u")),
                new OParam("end_at", end?.ToString("u"))
            });
        }

        public Task<Watching> Watching(string id = "me")
        {
            return MakeRequest<Watching>(new Request<Watching>($"users/{id}/watching/")
            {
                ExpectedStatusCode = new HttpStatusCode[] { HttpStatusCode.NoContent, HttpStatusCode.OK },
                ParseHandler = (resp) => resp switch
                {
                    OKResponse response => response.Convert<Watching>(),
                    NoContentResponse response => null,
                    _ => throw resp.ToException()
                }
            });
        }

        public Task<List<WrappedMediaObject>> GetList(string name, string user = "me") => MakeRequest<List<WrappedMediaObject>>($"users/{user}/lists/{name}/items/");
        public async Task<IEnumerable<MovieSmall>> GetListMovies(string name, string user = "me") => (await MakeRequest<List<WrappedMediaObject>>($"users/{user}/lists/{name}/items/movies")).Select(i => i.Movie);
        public async Task<IEnumerable<ShowSmall>> GetListShow(string name, string user = "me") => (await MakeRequest<List<WrappedMediaObject>>($"users/{user}/lists/{name}/items/show")).Select(i => i.Show);
        public async Task<IEnumerable<Show>> GetListShowExtended(string name, string user = "me") => (await MakeRequest<List<ExtendedWrappedMediaObject>>($"users/{user}/lists/{name}/items/show", parameters: new List<Param>{
            new Param("extended", "full")
        })).Select(i => i.Show);
        public async Task<IEnumerable<SeasonSmall>> GetListSeason(string name, string user = "me") => (await MakeRequest<List<WrappedMediaObject>>($"users/{user}/lists/{name}/items/season")).Select(i => i.Season);

        public Task AddList(string list, ShowSmall show, string user = "me") => AddList(list, new SyncRequestObject {
            shows = new List<Media> { show }
        }, user);
        public Task AddList(string list, SyncRequestObject requestObject, string user = "me") => MakeRequest<string>($"users/{user}/lists/{list}/items", Call.POST, content: requestObject, statusCode: System.Net.HttpStatusCode.Created);
    }
}

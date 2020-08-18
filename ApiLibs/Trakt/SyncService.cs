using ApiLibs.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiLibs.Trakt
{
    public class SyncService : SubService
    {
        public SyncService(TraktService service) : base(service)
        {
        }

        public Task AddToHistory(SyncRequestObject media) => MakeRequest<string>("/sync/history", Call.POST, content: media);
        public Task RemoveFromHistory(SyncRequestObject media) => MakeRequest<string>("/sync/history/remove", Call.POST, content: media);
        
        public Task AddRatings(SyncRequestObject media) => MakeRequest<string>("/sync/ratings", Call.POST, content: media);
        public Task RemoveFromRatings(SyncRequestObject media) => MakeRequest<string>("/sync/ratings/remove", Call.POST, content: media);

        public Task AddToWatchlist(SyncRequestObject media) => MakeRequest<string>("/sync/watchlist", Call.POST, content: media);
        public Task RemoveFromWatchlist(SyncRequestObject media) => MakeRequest<string>("/sync/watchlist/remove", Call.POST, content: media);

        public Task<List<WrappedMediaObject>> GetWatchlistMovies(string sort = null) => GetWatchlist<WrappedMediaObject>("movies", sort);
        public Task<List<WrappedMediaObject>> GetWatchlistEpisode(string sort = null) => GetWatchlist<WrappedMediaObject>("episodes", sort);
        private Task<List<T>> GetWatchlist<T>(string type, string sort = null) => MakeRequest<List<T>>($"/sync/watchlist/{type}", parameters: new List<Param> {
            new OParam("sort", sort)
        });

        public class SyncRequestObject {
            public List<MediaRequestObject> movies { get; set; }
            public List<MediaRequestObject> shows { get; set; }
            public List<MediaRequestObject> seasons { get; set; }
            public List<MediaRequestObject> episodes { get; set; }
        }
    }
}

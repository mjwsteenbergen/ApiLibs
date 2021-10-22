using ApiLibs.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiLibs.Trakt
{
    public class SyncService : SubService<TraktService>
    {
        public SyncService(TraktService service) : base(service)
        {
        }

        public Task AddToHistory(SyncRequestObject media) => MakeRequest<string>("/sync/history", Call.POST, content: media);
        public Task RemoveFromHistory(SyncRequestObject media) => MakeRequest<string>("/sync/history/remove", Call.POST, content: media);
        
        public Task AddRatings(SyncRequestObject media) => MakeRequest<string>("/sync/ratings", Call.POST, content: media, statusCode: System.Net.HttpStatusCode.Created);
        public Task RemoveFromRatings(SyncRequestObject media) => MakeRequest<string>("/sync/ratings/remove", Call.POST, content: media);

        public Task AddToWatchlist(SyncRequestObject media) => MakeRequest<string>("/sync/watchlist", Call.POST, content: media);
        public Task RemoveFromWatchlist(SyncRequestObject media) => MakeRequest<string>("/sync/watchlist/remove", Call.POST, content: media);

        public Task<List<WrappedMediaObject>> GetWatchlistMovies(string sort = null) => GetWatchlist<WrappedMediaObject>("movies", sort);
        public Task<List<WrappedMediaObject>> GetWatchlistEpisode(string sort = null) => GetWatchlist<WrappedMediaObject>("episodes", sort);
        public Task<List<WrappedMediaObject>> GetWatchlistSeasons(string sort = null) => GetWatchlist<WrappedMediaObject>("seasons", sort);
        public Task<List<WrappedMediaObject>> GetWatchlistShows(string sort = null) => GetWatchlist<WrappedMediaObject>("shows", sort);
        private Task<List<T>> GetWatchlist<T>(string type, string sort = null) => MakeRequest<List<T>>($"/sync/watchlist/{type}", parameters: new List<Param> {
            new OParam("sort", sort)
        });

        // public Task<List<PlaybackProgress>> GetPlaybackProgress(DateTime? start, DateTime? end, string type = "") => MakeRequest<List<PlaybackProgress>>($"sync/playback/{type}", parameters: new List<Param> {
        //     new OParam("start_at", start?.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffZ").Replace(" ", "T")),
        //     new OParam("end_at", end?.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffZ").Replace(" ", "T"))
        // });

        public Task DeletePlayback(string id) => MakeRequest<string>($"sync/playback/{id}", Call.DELETE);

        
    }
    
    public class SyncRequestObject
    {
        public List<Media> movies { get; set; }
        public List<Media> shows { get; set; }
        public List<Media> seasons { get; set; }
        public List<Media> episodes { get; set; }
    }
}

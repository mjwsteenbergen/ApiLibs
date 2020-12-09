using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiLibs.General;
using Martijn.Extensions.Linq;
using Martijn.Extensions.AsyncLinq;

namespace ApiLibs.Spotify
{
    public class LibraryService : SubService<SpotifyService>
    {
        public LibraryService(SpotifyService service) : base(service)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset">The index of the first object to return. Default: 0 (i.e., the first object). Use with limit to get the next set of objects.</param>
        /// <param name="limit">The maximum number of objects to return. Default: 20. Minimum: 1. Maximum: 50.</param>
        /// <param name="market">An ISO 3166-1 alpha-2 country code or the string from_token. Provide this parameter if you want to apply Track Relinking.</param>
        /// <returns></returns>
        public Task<MyTrackResponse> GetMySavedTracks(int? offset = null, int? limit = null, string market = null)
        {
            return MakeRequest<MyTrackResponse>("me/tracks", parameters: new List<Param>
            {
                new OParam("offset", offset),
                new OParam("limit", limit),
                new OParam("market", market)
            });
        }

        public async IAsyncEnumerable<SavedTrack> GetMySavedTracksAsync()
        {
            int offset = 0;
            MyTrackResponse res = null;
            do {
                res = await GetMySavedTracks(offset, 50);
                offset += 50;

                foreach (var item in res.Items)
                {
                    yield return item;
                }

                await Task.Delay(50);

            } while(res.Items.Count > 0);
            throw new System.NotImplementedException();
        }

        public Task SaveTracks(IEnumerable<Track> ids) => SaveTracks(ids.Select(i => i.Id));

        public Task SaveTracks(IEnumerable<string> ids)
        {
            return ids
                .Split(50)
                .Select(i => HandleRequest("me/tracks", Call.PUT, new List<Param> {
                    new Param("ids", i.Combine(","))
                })).ToIAsyncEnumberable().ToList();
        }

        public async Task SaveTrack(string id)
        {
            await SaveTracks(new List<string> { id });
        }

        public Task RemoveSavedTracks(IEnumerable<Track> ids) => RemoveSavedTracks(ids.Select(i => i.Id));

        public Task RemoveSavedTracks(IEnumerable<string> ids)
        {
            return ids 
                .Split(50)
                .Select(i => 
                    HandleRequest("me/tracks", Call.DELETE, new List<Param> 
                    {
                        new Param("ids", i.Combine(","))
                    })
                ).ToIAsyncEnumberable().ToList();
        }
    }
}

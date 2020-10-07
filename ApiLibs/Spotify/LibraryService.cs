using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiLibs.General;

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
        public Task<MyTrackResponse> GetMyTracks(int? offset = null, int? limit = null, string market = null)
        {
            return MakeRequest<MyTrackResponse>("me/tracks", parameters: new List<Param>
            {
                new OParam("offset", offset),
                new OParam("limit", limit),
                new OParam("market", market)
            });
        }
    }
}

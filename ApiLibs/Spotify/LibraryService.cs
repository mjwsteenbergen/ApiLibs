using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiLibs.General;

namespace ApiLibs.Spotify
{
    public class LibraryService : SubService
    {
        public LibraryService(SpotifyService service) : base(service)
        {

        }

        public Task<MyTrackResponse> GetMyTracks(int offset)
        {
            return MakeRequest<MyTrackResponse>("me/tracks", parameters: new List<Param>
            {
                new Param("offset", offset.ToString())
            });
        }
    }
}

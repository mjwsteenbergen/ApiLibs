using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiLibs.General;

namespace ApiLibs.Spotify
{
    public class ProfileService : SubService<SpotifyService>
    {
        public ProfileService(SpotifyService service) : base(service)
        {
        }

        public async Task<MeObject> GetMe()
        {
            return await MakeRequest<MeObject>("me");
        }

        //User

        public async Task Follow(UserType type, string id)
        {
            await Follow(type, new List<string>
            {
                id
            });
        }

        public async Task Follow(UserType type, List<string> ids)
        {
            await HandleRequest("me/following", Call.PUT, new List<Param>
            {
                new Param("type",type.ToString().ToLower()),
                new Param("ids", ids.Aggregate((i,j) => i + "," + j))
            }, statusCode: System.Net.HttpStatusCode.NoContent);
        }

        public async Task Unfollow(UserType type, string id)
        {
            await Unfollow(type, new List<string>
            {
                id
            });
        }

        public async Task Unfollow(UserType type, List<string> ids)
        {
            await HandleRequest("me/following", Call.DELETE, new List<Param>
            {
                new Param("type",type.ToString().ToLower()),
                new Param("ids", ids.Aggregate((i,j) => i + "," + j))
            }, statusCode: System.Net.HttpStatusCode.NoContent);
        }

        public async Task<bool> CheckIfFollowing(UserType type, List<string> ids)
        {
            string output = (await HandleRequest("me/following/contains", parameters: new List<Param>
            {
                new Param("type", type.ToString().ToLower()),
                new Param("ids", ids.Aggregate((i, j) => i + "," + j))
            }));
            return bool.Parse(output.Replace("[", "").Replace("]", ""));
        }

        public async Task<ArtistResultsResponse> GetFollowingArtists(int? limit = null, string after = null)
        {
            return (await MakeRequest<SearchObject>("me/following", parameters: new List<Param>
            {
                new Param("type", "artist"),
                new OParam("limit", limit),
                new OParam("after", after)
            })).artists;
        }

        public async IAsyncEnumerable<Artist> GetFollowingArtistsAsync()
        {
            Artist last = null;
            ArtistResultsResponse artistsResponse = null;
            do {
                artistsResponse = await GetFollowingArtists(50, last?.Id);
                foreach (var item in artistsResponse.items)
                {
                    last = item;
                    yield return item;
                }
            } while(artistsResponse.items.Count > 0);
            // var first = await GetFollowingArtists();


            // foreach(var item in first.items)
            // {
            //     last = item;
            //     yield return item;
            // }

            // for (int i = 0; i < first.Total; i += first.Limit)
            // {
                
            // }
        }

        public async Task<List<Artist>> GetTopArtists()
        {
            return (await MakeRequest<ArtistResultsResponse>("me/top/tracks")).items;
        }

        //Tracks

        public async Task<List<Track>> GetTopTracks()
        {
            return (await MakeRequest<TrackResultsResponse>("me/top/tracks")).items;
        }

        public async Task<List<Track>> GetRecentlyPlayed()
        {
            return (await MakeRequest<TrackResultsResponse>("me/player/recently-played")).items;
        }
    }
}

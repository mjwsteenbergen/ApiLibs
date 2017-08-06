using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiLibs.General;

namespace ApiLibs.Spotify
{
    public class ProfileService : SubService
    {
        public ProfileService(Service service) : base(service)
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
            })).Content;
            return bool.Parse(output.Replace("[", "").Replace("]", ""));
        }

        public async Task<List<Artist>> GetFollowingArtists()
        {
            return (await MakeRequest<ArtistResultsResponse>("me/following")).items;
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

        public async Task<List<Track>> GetSavedTracks()
        {
            return (await MakeRequest<TrackResultsResponse>("me/tracks")).items;
        }

        public async Task SaveTracks(List<string> ids)
        {
            await HandleRequest("me/tracks" + ids.Aggregate((i,j) => i + "," + j), Call.PUT);
        }

        public async Task SaveTrack(string id)
        {
            await SaveTracks(new List<string> {id});
        }
    }
}

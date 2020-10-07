using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiLibs.General;

namespace ApiLibs.Spotify
{
    public class AlbumService : SubService<SpotifyService>
    {
        public AlbumService(SpotifyService spotify) : base(spotify) { }

        public async Task<Album> GetAlbum(string id)
        {
            return await MakeRequest<Album>("albums/" + id);
        }

        public async Task<List<Album>> GetNewReleases()
        {
            return await MakeRequest<List<Album>>("/browse/new-releases");
        }

        public async Task<List<Track>> GetTracks(Album album)
        {
            return await GetTracks(album.Id);
        }

        public async Task<List<Track>> GetTracks(string albumId)
        {
            return (await MakeRequest<TrackResultsResponse>("/albums/" + albumId + "/tracks")).items;
        }
    }
}

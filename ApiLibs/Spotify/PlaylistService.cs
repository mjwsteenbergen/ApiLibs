using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ApiLibs.General;

namespace ApiLibs.Spotify
{
    public class PlaylistService : SubService
    {
        public PlaylistService(SpotifyService service) : base(service) { }

        public async Task<PlaylistRoot> GetMyPlaylists()
        {
            return await MakeRequest<PlaylistRoot>("me/playlists");
        }

        public async Task DeletePlaylist(string ownerId, string playlistId)
        {
            await HandleRequest($"users/{ownerId}/playlists/{playlistId}/followers", Call.DELETE);
        }

        public async Task DeletePlaylist(Playlist playlist)
        {
            await DeletePlaylist(playlist.Owner.Id, playlist.Id);
        }
    }
}

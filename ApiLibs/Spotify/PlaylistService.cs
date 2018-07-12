using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiLibs.General;

namespace ApiLibs.Spotify
{
    public class PlaylistService : SubService
    {
        public PlaylistService(SpotifyService service) : base(service) { }

        public async Task<PlaylistResultsResponse> GetMyPlaylists()
        {
            return await MakeRequest<PlaylistResultsResponse>("me/playlists");
        }

        public async Task DeletePlaylist(string ownerId, string playlistId)
        {
            await HandleRequest($"users/{ownerId}/playlists/{playlistId}/followers", Call.DELETE);
        }

        public async Task DeletePlaylist(Playlist playlist)
        {
            await DeletePlaylist(playlist.Owner.Id, playlist.Id);
        }

        public Task<Playlist> CreatePlaylist(string owner, string name, bool @public = false, bool collaborative = false)
        {
            return MakeRequest<Playlist>($"users/{owner}/playlists", Call.POST, content: new
            {
                name,
                @public,
                collaborative
            });
        }

        public async Task AddTracks(IEnumerable<Track> tracks, Playlist playlist)
        {
            await AddTracks(tracks.Select(i => i.Uri), playlist.Id, playlist.Owner.Id);
        }

        public async Task AddTracks(IEnumerable<string> tracks, string playlistId, string owner)
        {
            await HandleRequest($"users/{owner}/playlists/{playlistId}/tracks", Call.POST, content: tracks);
        }


        public async Task<PlaylistTrackResponse> GetTracks(Playlist playlist)
        {
            return await GetTracks(playlist.Id, playlist.Owner.Id);
        }

        private async Task<PlaylistTrackResponse> GetTracks(string playlistId, string ownerId)
        {
            return await MakeRequest<PlaylistTrackResponse>($"users/{ownerId}/playlists/{playlistId}/tracks");
        }

        public async Task RemoveTracks(Playlist playlist, IEnumerable<Track> tracks)
        {
            await RemoveTracks(playlist.Id, playlist.Owner.Id, tracks.Select(i => i.Uri));
        }

        private async Task RemoveTracks(string playlistId, string ownerId, IEnumerable<string> tracks)
        {
            await HandleRequest($"users/{ownerId}/playlists/{playlistId}/tracks", Call.DELETE, content: new
            {
                tracks = tracks.Select(i => new
                {
                    uri = i
                })
            });
        }
    }
}

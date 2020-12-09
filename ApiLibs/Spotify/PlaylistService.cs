using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiLibs.General;

namespace ApiLibs.Spotify
{
    public class PlaylistService : SubService<SpotifyService>
    {
        public PlaylistService(SpotifyService service) : base(service) { }

        public async Task<PlaylistResultsResponse> GetMyPlaylists(int offset = 0, int limit = 50)
        {
            return await MakeRequest<PlaylistResultsResponse>("me/playlists", parameters: new List<Param> {
                new Param(nameof(offset), offset),
                new Param(nameof(limit), limit),
            });
        }

        public async IAsyncEnumerable<Playlist> GetMyPlaylistsAsync()
        {
            PlaylistResultsResponse res = null;
            int offset = 0;
            do
            {
                res = await GetMyPlaylists(offset);
                offset += 50;

                foreach (var item in res.Items)
                {
                    yield return item;
                }

            } while(res.Items.Count != 0);
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
            tracks = tracks.ToList();
            for (int i = 0; i < tracks.Count(); i+=100)
            {
                await AddTracksSingleCall(tracks.Select(j => j.Uri).Skip(i).Take(100), playlist.Id, playlist.Owner.Id);
            }
        }

        public async Task AddTracksSingleCall(IEnumerable<Track> tracks, Playlist playlist)
        {
            await AddTracksSingleCall(tracks.Select(i => i.Uri), playlist.Id, playlist.Owner.Id);
        }

        public async Task AddTracksSingleCall(IEnumerable<string> tracks, string playlistId, string owner)
        {
            await HandleRequest($"users/{owner}/playlists/{playlistId}/tracks", Call.POST, content: tracks);
        }

        public async Task<List<Track>> GetAllTracks(Playlist playlist)
        {
            List<Track> tracks = new List<Track>();
            var playlistTracks = await GetTracks(playlist);
            tracks.AddRange(playlistTracks.Items.Select(i => i.Track));

            if (playlistTracks.Total > 100)
            {
                for (int i = 1; i < (int) Math.Ceiling(playlistTracks.Total / 100.0) ; i++)
                {
                    tracks.AddRange((await GetTracks(playlist, offset: i * 100)).Items.Select(j => j.Track));
                }
            }

            return tracks;
        }

        public async Task<PlaylistTrackResponse> GetTracks(Playlist playlist, int? limit = null, int? offset = null)
        {
            return await GetTracks(playlist.Id, playlist.Owner.Id, limit, offset);
        }

        private async Task<PlaylistTrackResponse> GetTracks(string playlistId, string ownerId, int? limit = null, int? offset = null)
        {
            return await MakeRequest<PlaylistTrackResponse>($"users/{ownerId}/playlists/{playlistId}/tracks", parameters: new List<Param>
            {
                new OParam("offset", offset),
                new OParam("limit", limit)
            });
        }

        public async Task RemoveTracksAll(Playlist playlist, IEnumerable<Track> tracks)
        {
            tracks = tracks.Where(i => i != null).ToList();
            while(tracks.Count() > 0)
            {
                var remove = tracks.Take(100);
                await RemoveTracks(playlist.Id, playlist.Owner.Id, remove.Select(i => i.Uri));
                tracks = tracks.Skip(100).ToList();
            }
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

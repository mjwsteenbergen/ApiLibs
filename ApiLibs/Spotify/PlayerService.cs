using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ApiLibs.General;

namespace ApiLibs.Spotify
{
    public class PlayerService : SubService
    {
        public PlayerService(SpotifyService service) : base(service) { }


        /// <summary>
        /// Get information about a user’s available devices.
        /// </summary>
        /// <returns><see cref="DeviceList"/></returns>
        public async Task<DeviceList> GetDevices()
        {
            return await MakeRequest<DeviceList>("me/player/devices");
        }

        /// <summary>
        /// Get information about the user’s current playback state, including track, track progress, and active device.
        /// </summary>
        /// <returns><see cref="PlaybackState"/></returns>
        public async Task<PlaybackState> GetPlayer()
        {
            return await MakeRequest<PlaybackState>("me/player");
        }

        /// <summary>
        /// Get the object currently being played on the user’s Spotify account.
        /// </summary>
        /// <returns><see cref="CurrentlyPlayingState"/></returns>
        public async Task<CurrentlyPlayingState> GetCurrentPlaying()
        {
            return await MakeRequest<CurrentlyPlayingState>("me/player/currently-playing");
        }

        /// <summary>
        /// Transfer playback to a new device and determine if it should start playing.
        /// </summary>
        /// <param name="d">the device on which playback should be started/transferred.</param>
        /// <param name="forcePlayback">force if the device should start playing</param>
        /// <returns></returns>
        public async Task TransferPlayback(Device d, bool forcePlayback)
        {
            await TransferPlayback(d.id);
        }

        /// <summary>
        /// Transfer playback to a new device and determine if it should start playing.
        /// </summary>
        /// <param name="id">the ID of the device on which playback should be started/transferred.</param>
        /// <returns></returns>
        public async Task TransferPlayback(string id)
        {
            Dictionary<string, object> kvp = new Dictionary<string, object>();
            kvp.Add("device_ids", new[] {id}); 
            await Service.HandleRequest("me/player", Call.PUT, content: kvp, statusCode: System.Net.HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Start a new context or resume current playback on the user’s active device.
        /// </summary>
        /// <param name="track">The track you want to play</param>
        /// <param name="device">on which device you want to play it</param>
        /// <returns></returns>
        public async Task Play(Track track, Device device = null)
        {
            await Play(new List<Track> {track}, device);
        }

        /// <summary>
        /// Start a new context or resume current playback on the user’s active device.
        /// </summary>
        /// <param name="tracks">A list of tracks you want to play</param>
        /// <param name="device">on which device you want to play it</param>
        /// <returns></returns>
        public async Task Play(List<Track> tracks, Device device)
        {
            await Play(tracks.ConvertAll(i => i.Id), device?.id);
        }

        /// <summary>
        /// Start a new context or resume current playback on the user’s active device.
        /// </summary>
        /// <param name="trackIds">The ids of the tracks you want to play</param>
        /// <param name="deviceId">on which device you want to play it</param>
        /// <returns></returns>
        public async Task Play(List<string> trackIds, string deviceId = null)
        {
            Dictionary<string, string[]> kvp = new Dictionary<string, string[]>
            {
                {"uris", trackIds.ConvertAll<string>(i => "spotify:track:" + i).ToArray()}
            };
            deviceId = deviceId == null ? "" : "?device_id = " + deviceId;
            await Service.HandleRequest("me/player/play" + deviceId, Call.PUT, new List<Param>
            {
//                new Param("device_id", deviceId)
            }, content: kvp, statusCode: System.Net.HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Start a new context or resume current playback on the user’s active device.
        /// </summary>
        /// <param name="album">The album you want to play</param>
        /// <param name="deviceId">on which device you want to play it</param>
        /// <returns></returns>
        public async Task Play(Album album, string deviceId = null)
        {
            await Play(album.Id, "album", deviceId);
        }

        /// <summary>
        /// Start a new context or resume current playback on the user’s active device.
        /// </summary>
        /// <param name="artist">The artist you want to play</param>
        /// <param name="deviceId">on which device you want to play it</param>
        /// <returns></returns>
        public async Task Play(Artist artist, string deviceId = null)
        {
            await Play(artist.Id, "artist", deviceId);
        }

        /// <summary>
        /// Start a new context or resume current playback on the user’s active device.
        /// </summary>
        /// <param name="playlist">The playlist you want to play</param>
        /// <param name="deviceId">on which device you want to play it</param>
        /// <returns></returns>
        public async Task Play(Playlist playlist, string deviceId = null)
        {
            await Play(playlist.Id, "user:" + playlist.Owner.Id + ":playlist", deviceId);
        }

        internal async Task Play(string contextUri, string type, string deviceId = null)
        {
            try
            {
                if (await (Service as SpotifyService).IsPremiumUser())
                {
                    PlayContext kvp = new PlayContext {context_uri = "spotify:" + type + ":" + contextUri};
                    deviceId = deviceId == null ? "" : "?device_id = " + deviceId;
                    await Service.HandleRequest("me/player/play" + deviceId, Call.PUT, content: kvp,
                        statusCode: HttpStatusCode.NoContent);
                }
            }
            catch (PageNotFoundException) { }
        }

        class PlayContext
        {
            public string context_uri { get; set; }
        }

        /// <summary>
        /// Pause playback on the user’s account.
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public async Task Pause(string deviceId = null)
        {
            deviceId = deviceId == null ? "" : "?device_id = " + deviceId;
            await Service.HandleRequest("me/player/pause" + deviceId, Call.PUT, statusCode: System.Net.HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Skips to next track in the user’s queue.
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public async Task Next(string deviceId = null)
        {
            List<Param> param = new List<Param>();
            if (deviceId != null)
            {
                param.Add(new Param("device_id", deviceId));
            }
            await HandleRequest("me/player/next", Call.POST, param, statusCode: System.Net.HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Skip User’s Playback To Previous Track. Note that this will ALWAYS skip to the previous track, regardless of the current track’s progress.
        /// </summary>
        /// <param name="deviceId">The id of the device this command is targeting. If not supplied, the user's currently active device is the target.</param>
        /// <returns></returns>
        public async Task Previous(string deviceId = null)
        {
            List<Param> param = new List<Param>();
            if (deviceId != null)
            {
                param.Add(new Param("device_id", deviceId));
            }
            await HandleRequest("me/player/previous", Call.POST, param, statusCode: System.Net.HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Seeks to the given position in the user’s currently playing track.
        /// </summary>
        /// <param name="position">The position in milliseconds to seek to. Must be a positive number. Passing in a position that is greater than the length of the track will cause the player to start playing the next song.</param>
        /// <param name="deviceId">The id of the device this command is targeting. If not supplied, the user's currently active device is the target.</param>
        /// <returns></returns>
        public async Task Seek(int position, string deviceId = null)
        {
            await HandleRequest("me/player/seek", Call.PUT, new List<Param>
            {
                new Param("position_ms", position),
                new OParam("device_id", deviceId)
            }, statusCode: System.Net.HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Set the repeat mode for the user’s playback. 
        /// </summary>
        /// <param name="state">track, context or off. track will repeat the current track. context will repeat the current context. off will turn repeat off.</param>
        /// <param name="deviceId">The id of the device this command is targeting. If not supplied, the user's currently active device is the target.</param>
        /// <returns></returns>
        public async Task Repeat(RepeatState state, string deviceId = null)
        {
            List<Param> param = new List<Param>
            {
                new Param("state", state.ToString().ToLower())
            };
            if (deviceId != null)
            {
                param.Add(new Param("device_id", deviceId));
            }
            await HandleRequest("me/player/repeat", Call.PUT, param, statusCode: System.Net.HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Set the volume for the user’s current playback device.
        /// </summary>
        /// <param name="volume_percent">Integer. The volume to set. Must be a value from 0 to 100 inclusive.</param>
        /// <param name="deviceId">The id of the device this command is targeting. If not supplied, the user's currently active device is the target.</param>
        /// <returns></returns>
        public async Task SetVolume(int volume_percent, string deviceId = null)
        {
            List<Param> param = new List<Param>
            {
                new Param("volume_percent", volume_percent)
            };
            if (deviceId != null)
            {
                param.Add(new Param("device_id", deviceId));
            }
            await HandleRequest("me/player/volume", Call.PUT, param, statusCode: System.Net.HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Set the state of shuffle
        /// </summary>
        /// <param name="state">True for shuffling, False for not shuffling</param>
        /// <param name="device_id">The id of the device this command is targeting.</param>
        /// <returns></returns>
        public async Task Shuffle(bool state, string device_id = null)
        {
            try
            {
                if (await (Service as SpotifyService).IsPremiumUser())
                {
                    await HandleRequest("me/player/shuffle", Call.PUT, new List<Param>
                    {
                        new Param("state", state),
                        new OParam("device_id", device_id)
                    }, statusCode: HttpStatusCode.NoContent);
                }
            }
            catch (PageNotFoundException)
            {

            }
        }
    }

    public enum RepeatState
    {
        Track, Context, Off
    }
}

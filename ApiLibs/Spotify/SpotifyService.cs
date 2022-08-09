using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ApiLibs.General;
using RestSharp;

namespace ApiLibs.Spotify
{
    public class SpotifyService : RestSharpService
    {
        private string _RefreshToken;
        private string _ClientBase64;

        private bool? premiumUser = null;

        public TrackService TrackService { get; }
        public AlbumService AlbumService { get; }
        public PlayerService PlayerService { get; }
        public ArtistService ArtistService { get; }
        public ProfileService ProfileService { get; }
        public LibraryService LibraryService { get; }
        public PlaylistService PlaylistService { get; }

        public class RefreshArgs : EventArgs
        {
            public string RefreshToken { get; set; }
        }

        public SpotifyService() : base("https://accounts.spotify.com/") { }

        public SpotifyService(string refreshToken, string clientId, string clientSecret) : base("https://api.spotify.com/v1/")
        {
            _ClientBase64 = Base64Encode(clientId + ":" + clientSecret);
            _RefreshToken = refreshToken;

            TrackService = new TrackService(this);
            AlbumService = new AlbumService(this);
            PlayerService = new PlayerService(this);
            ArtistService = new ArtistService(this);
            ProfileService = new ProfileService(this);
            LibraryService = new LibraryService(this);
            PlaylistService = new PlaylistService(this);

            RequestResponseMiddleware.Add(async (resp) =>
            {
                if (resp.StatusCode == HttpStatusCode.BadRequest)
                {
                    var request = resp.Request;
                    if (request.EndPoint == "api/token")
                    {
                        return resp;
                    }
                    await RefreshToken();
                    request.Retried++;
                    return await base.HandleRequest(request);
                }
                return resp;
            });

            RequestResponseMiddleware.Add(async (resp) =>
            {
                if((int)resp.StatusCode == 429)
                {
                    var request = resp.Request;
                    await Task.Delay(2000);
                    request.Retried++;
                    return await base.HandleRequest(request);
                }
                return resp;
            });

            AddStandardHeader("Authorization", "To be filled in later");
        }

        public void Connect(IOAuth auth, string clientId, string redirectUrl, List<Scope> scopes, bool showDialog = false)
        {
            auth.ActivateOAuth("https://accounts.spotify.com/authorize?response_type=code" + "&client_id=" + clientId + "&redirect_uri=" +
                               redirectUrl + "&scope=" + string.Join(" ", scopes.Select(i => i.Value)) + "&show_dialog=" + showDialog);
        }

        private string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public async Task<AccessTokenObject> ConvertToToken(string code, string redirectUrl, string clientId, string clientSecret)
        {

            AddStandardHeader("Authorization", "Basic " + Base64Encode(clientId + ":" + clientSecret));

            return await MakeRequest<AccessTokenObject>("api/token", Call.POST, new List<Param>
            {
                new Param("grant_type", "authorization_code"),
                new Param("code", code),
                new Param("redirect_uri", redirectUrl)
            });
        }

        public async Task RefreshToken()
        {
            var args = await new BlandService().MakeRequest<AccessTokenObject>("https://accounts.spotify.com/api/token", Call.POST, new List<Param>
            {
                new Param("grant_type", "refresh_token"),
                new Param("refresh_token", _RefreshToken)
            }, headers: new List<Param>() {
                new Param("Authorization", "Basic " + _ClientBase64)
            });

            AddStandardHeader("Authorization", "Bearer " + args.access_token);
        }

        public async Task<SearchObject> Search(string query, SearchType type, int limit = 20, int offset = 0)
        {
            return await MakeRequest<SearchObject>("search", parameters: new List<Param>
            {
                new Param("q", query),
                new Param("type", type.ToString().ToLower()),
                new Param("limit", limit),
                new Param("offset", offset)
            });
        }

        public enum SearchType
        {
            Album, Artist, Playlist, Track
        }

        public async Task<bool> IsPremiumUser()
        {
            premiumUser = premiumUser ?? (await ProfileService.GetMe()).product == "premium";
            return premiumUser.Value;
        }

        public async Task<string> GetUserProfile()
        {
            var res = await MakeRequest<string>("me");

            return res;
        }

    }

    public class Scope
    {
        private Scope(string name)
        {
            Value = name;
        }

        public string Value { get; }

        public static Scope PlaylistReadPrivate => new Scope("playlist-read-private");
        public static Scope PlaylistReadCollaborative => new Scope("playlist-read-collaborative");
        public static Scope PlaylistModifyPublic => new Scope("playlist-modify-public");
        public static Scope PlaylistModifyPrivate => new Scope("playlist-modify-private");
        public static Scope Streaming => new Scope("streaming");
        public static Scope UserFollowModify => new Scope("user-follow-modify");
        public static Scope UserFollowRead => new Scope("user-follow-read");
        public static Scope UserLibraryRead => new Scope("user-library-read");
        public static Scope UserLibraryModify => new Scope("user-library-modify");
        public static Scope UserReadPrivate => new Scope("user-read-private");
        public static Scope UserReadBirthday => new Scope("user-read-birthdate");
        public static Scope UserReadEmail => new Scope("user-read-email");
        public static Scope UserTopRead => new Scope("user-top-read");
        public static Scope UserModifyPlaybackState => new Scope("user-modify-playback-state");
        public static Scope UserReadPlaybackState => new Scope("user-read-playback-state");
    }
}

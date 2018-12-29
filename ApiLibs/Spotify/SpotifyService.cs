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
    public class SpotifyService : Service
    {
        private string _RefreshToken;
        private string _ClientBase64;
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

            AddStandardHeader("Authorization", "To be filled in later");
        }

        public void Connect(IOAuth auth, string clientId, string redirectUrl, List<Scope> scopes, bool showDialog = false)
        {
            auth.ActivateOAuth("https://accounts.spotify.com/authorize?response_type=code" + "&client_id=" + clientId + "&redirect_uri=" +
                               redirectUrl + "&scope=" + String.Join(" ", scopes.Select(i => i.Value)) + "&show_dialog=" + showDialog);
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

        protected internal override async Task<string> HandleRequest(string url, Call call = Call.GET, List<Param> parameters = null, List<Param> headers = null, object content = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            try
            {
                return await base.HandleRequest(url, call, parameters, headers, content, statusCode);
            }
            catch (BadRequestException)
            {
                if (url == "api/token")
                {
                    throw;
                }
                await RefreshToken();
                return await base.HandleRequest(url, call, parameters, headers, content, statusCode);
            }
        }

        public async Task RefreshToken()
        {
            SetBaseUrl("https://accounts.spotify.com/");
            UpdateHeaderIfExists("Authorization", "Basic " + _ClientBase64);

            var args = await MakeRequest<AccessTokenObject>("api/token", Call.POST, new List<Param>
            {
                new Param("grant_type", "refresh_token"),
                new Param("refresh_token", _RefreshToken)
            });

            SetBaseUrl("https://api.spotify.com/v1/");
            UpdateHeaderIfExists("Authorization", "Bearer " + args.access_token);
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

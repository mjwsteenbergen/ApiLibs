using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiLibs.General;

namespace ApiLibs.Instapaper.V2
{
    /// <summary>
    /// Client for the Instapaper API v2.
    /// <see href="https://www.instapaper.com/developers/overview/introduction"/>
    /// </summary>
    public class InstapaperV2Service : RestSharpService
    {
        private const string HostUrl = "https://www.instapaper.com";
        private readonly string clientId;
        private readonly string clientSecret;

        public BookmarkService BookmarkService { get; }
        public FolderService FolderService { get; }
        public TagService TagService { get; }
        public HighlightService HighlightService { get; }

        /// <summary>
        /// Use this with a personal access token, or one issued through the OAuth flow.
        /// Personal access tokens are generated on your application's page at https://www.instapaper.com/developers/applications
        /// </summary>
        public InstapaperV2Service(string accessToken) : this()
        {
            SetAccessToken(accessToken);
        }

        /// <summary>
        /// Use this to authorize other users through the OAuth 2 authorization code flow.
        /// Call <see cref="Connect"/> and then <see cref="ExchangeCode"/>.
        /// </summary>
        /// <param name="clientId">The client id from your application's page</param>
        /// <param name="clientSecret">The client secret from your application's page</param>
        public InstapaperV2Service(string clientId, string clientSecret) : this()
        {
            this.clientId = clientId;
            this.clientSecret = clientSecret;
        }

        private InstapaperV2Service() : base(HostUrl + "/api/2/")
        {
            BookmarkService = new BookmarkService(this);
            FolderService = new FolderService(this);
            TagService = new TagService(this);
            HighlightService = new HighlightService(this);
        }

        private void SetAccessToken(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                throw new ArgumentException("accessToken is required", nameof(accessToken));
            }
            AddStandardHeader("Authorization", $"Bearer {accessToken}");
        }

        /// <summary>
        /// The URL to send the user to so they can authorize your application.
        /// </summary>
        /// <param name="redirectUri">Must match one of the application's registered callback URIs exactly</param>
        /// <param name="state">An opaque value echoed back to your redirect URI. Use it to defend against CSRF.</param>
        public string GetAuthorizationUrl(string redirectUri, string state = null)
        {
            var url = $"{HostUrl}/oauth2/authorize?client_id={Uri.EscapeDataString(clientId)}&redirect_uri={Uri.EscapeDataString(redirectUri)}&response_type=code";
            if (state != null)
            {
                url += $"&state={Uri.EscapeDataString(state)}";
            }
            return url;
        }

        /// <summary>
        /// Opens the authorization page for the user.
        /// </summary>
        public void Connect(IOAuth authenticator, string state = null)
        {
            authenticator.ActivateOAuth(new Uri(GetAuthorizationUrl(authenticator.RedirectUrl, state)));
        }

        /// <summary>
        /// Exchange the code from your redirect URI for an access token. Codes work once.
        /// Access tokens don't expire. This service uses the token for any further calls.
        /// </summary>
        public async Task<InstapaperAccessToken> ExchangeCode(string code, string redirectUri)
        {
            var token = await new BlandService().MakeRequest<InstapaperAccessToken>($"{HostUrl}/oauth2/token", Call.POST, new List<Param>
            {
                new Param("client_id", clientId),
                new Param("client_secret", clientSecret),
                new Param("redirect_uri", redirectUri),
                new Param("code", code),
            });
            SetAccessToken(token.AccessToken);
            return token;
        }

        /// <summary>
        /// The account the access token belongs to.
        /// </summary>
        public Task<InstapaperUser> GetMe() => MakeRequest<InstapaperUser>("me");

        internal static long CheckId(long id, string name = "id")
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(name, id, "Expected a positive ID");
            }
            return id;
        }
    }
}

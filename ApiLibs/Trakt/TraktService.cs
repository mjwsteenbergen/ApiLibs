using ApiLibs.General;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ApiLibs.Trakt
{
    public class TraktService : Service
    {
        private string accessToken;
        private string refreshToken;
        private readonly string clientId;
        private readonly string clientSecret;
        private readonly string redirectUrl;

        public Func<AccessObject, Task> StoreRefreshToken;

        public UserService UserService { get; private set; }
        public SyncService SyncService { get; private set; }
        public MovieService MovieService { get; private set; }
        public SearchService SearchService { get; private set; }
        public ShowService ShowService { get; }

        public TraktService() : base("https://api.trakt.tv")
        {
            UserService = new UserService(this);
            SyncService = new SyncService(this);
            MovieService = new MovieService(this);
            SearchService = new SearchService(this);
            ShowService = new ShowService(this);
        }

        public TraktService(string accessToken, string refreshToken, string clientId, string clientSecret, string redirectUrl) : this()
        {
            this.accessToken = accessToken;
            this.refreshToken = refreshToken;
            this.clientId = clientId;
            this.clientSecret = clientSecret;
            this.redirectUrl = redirectUrl;
            AddStandardHeader("Authorization", $"Bearer {accessToken}");
            AddStandardHeader("trakt-api-key", clientId);
            AddStandardHeader("trakt-api-version", "2");
        }

        public void Connect(IOAuth auth, string clientId, string redirectUrl)
        {
            auth.ActivateOAuth($"https://trakt.tv/oauth/authorize?response_type=code&client_id={clientId}&redirect_uri={redirectUrl}");
        }

        public Task<AccessObject> ConvertToToken(string code, string clientId, string clientSecret, string redirectUrl)
        {
            return MakeRequest<AccessObject>("oauth/token", Call.POST, new List<Param>
            {
                new Param("code", code),
                new Param("client_id", clientId),
                new Param("client_secret", clientSecret),
                new Param("redirect_uri", redirectUrl),
                new Param("grant_type", "authorization_code")
            });
        }

        protected internal override async Task<string> HandleRequest(string url, Call call = Call.GET,
            List<Param> parameters = null, List<Param> headers = null, object content = null,
            HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            try
            {
                return await base.HandleRequest(url, call, parameters, headers, content, statusCode);
            }
            catch (UnAuthorizedException)
            {
                if(url == "oauth/token")
                {
                    throw;
                }
                if(StoreRefreshToken != null)
                {
                    var res = await RefreshAccessToken();
                    await StoreRefreshToken(res);
                    return await base.HandleRequest(url, call, parameters, headers, content, statusCode);
                } 
                else
                {
                    throw;
                }
            }

        }

        public async Task<AccessObject> RefreshAccessToken()
        {
           RemoveStandardHeader("Authorization");

            var obj = await MakeRequest<AccessObject>("oauth/token", Call.POST,
                content: new
                {
                    refresh_token = refreshToken,
                    client_id = clientId,
                    client_secret = clientSecret,
                    redirect_uri = redirectUrl,
                    grant_type = "refresh_token"
                });
            this.accessToken = obj.AccessToken;
            this.refreshToken = obj.RefreshToken;
            AddStandardHeader("Authorization", $"Bearer {obj.AccessToken}");
            return obj;
        }
    }
}

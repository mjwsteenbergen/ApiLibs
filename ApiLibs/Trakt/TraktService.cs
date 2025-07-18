using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ApiLibs.Trakt
{
    public class TraktService : RestSharpService
    {
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

        public TraktService(string clientId, string clientSecret, string redirectUrl) : base("https://api.trakt.tv")
        {
            this.clientId = clientId;
            this.clientSecret = clientSecret;
            this.redirectUrl = redirectUrl;

            UserService = new UserService(this);
            SyncService = new SyncService(this);
            MovieService = new MovieService(this);
            SearchService = new SearchService(this);
            ShowService = new ShowService(this);

            RequestResponseMiddleware.Add(async (resp) =>
            {
                var request = resp.Request;
                if ((resp.StatusCode == HttpStatusCode.BadRequest || resp.StatusCode == HttpStatusCode.Unauthorized) && request.EndPoint != "oauth/token" && StoreRefreshToken != null)
                {
                    var res = await RefreshAccessToken();
                    await StoreRefreshToken(res);
                    request.Retried++;
                    return await base.HandleRequest(request);
                }

                return resp;
            });
        }

        public TraktService(string accessToken, string refreshToken, string clientId, string clientSecret, string redirectUrl) : this(clientId, clientSecret, redirectUrl)
        {
            this.refreshToken = refreshToken;
            AddStandardHeader("Authorization", $"Bearer {accessToken}");
            AddStandardHeader("trakt-api-key", clientId);
            AddStandardHeader("trakt-api-version", "2");
        }

        public void Connect(IOAuth auth)
        {
            auth.ActivateOAuth($"https://trakt.tv/oauth/authorize?response_type=code&client_id={clientId}&redirect_uri={redirectUrl}");
        }

        public Task<AccessObject> ConvertToToken(string code)
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
            refreshToken = obj.RefreshToken;
            AddStandardHeader("Authorization", $"Bearer {obj.AccessToken}");
            return obj;
        }
    }
}

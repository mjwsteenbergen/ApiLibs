using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiLibs.Reddit;
using RestSharp;
using RestSharp.Authenticators;

namespace ApiLibs.Folder
{
    public class RedditService : Service
    {
        private string Refreshtoken { get; }
        private string ClientSecret { get; }
        private readonly string _user;
        private readonly string _clientId;

        public RedditService(string redditToken, string refreshtoken, string clientId, string clientSecret, string user)
        {
            Refreshtoken = refreshtoken;
            ClientSecret = clientSecret;
            _clientId = clientId;
            SetUp("https://oauth.reddit.com");
            _user = user;
            AddStandardHeader(new Param("Authorization", "bearer " + redditToken));
        }

        public void Connect(IOAuth oAuth, string redirectUrl, List<string> scopes, string randomString,  string duration = "permanent")
        {
            oAuth.ActivateOAuth(
                new Uri(
                    "https://www.reddit.com/api/v1/authorize?client_id=" + _clientId + "&response_type=code&state=" + randomString + "&redirect_uri=" +
                    redirectUrl + "&duration=" + duration + "&scope=" + scopes.Aggregate((i, j) => i + " " + j)));
        }

        public async Task<TokenObject> GetAccessToken(string code, string redirectUrl)
        {
            SetBaseUrl("https://www.reddit.com/");
            RemoveStandardHeader("Authorization");
            List<Param> parameters = new List<Param>
            {
                new Param("grant_type","authorization_code"),
                new Param("code", code),
                new Param("redirect_uri", redirectUrl)
            };
            var returns = await MakeRequest<TokenObject>("api/v1/access_token", Call.POST, parameters, new List<Param> { new Param("Authorization", "Basic " + System.Convert.ToBase64String(Encoding.UTF8.GetBytes(_clientId + ":" + ClientSecret))) });
            SetBaseUrl("https://oauth.reddit.com");
            return returns;
        }

        public async Task RefreshToken()
        {
            SetBaseUrl("https://www.reddit.com/");
            RemoveStandardHeader("Authorization");
            List<Param> parameters = new List<Param>
            {
                new Param("grant_type","refresh_token"),
                new Param("refresh_token", Refreshtoken),
            };

            var returns = await MakeRequest<TokenObject>("api/v1/access_token", Call.POST, parameters, new List<Param> { new Param("Authorization", "Basic " + System.Convert.ToBase64String(Encoding.UTF8.GetBytes(_clientId + ":" + ClientSecret))) });
            SetBaseUrl("https://oauth.reddit.com");
            AddStandardHeader("Authorization", "bearer " + returns.access_token);
        }

        internal override async Task<IRestResponse> HandleRequest(string url, Call call = Call.GET, List<Param> parameters = null, List<Param> headers = null, object content = null)
        {
            try
            {
                return await base.HandleRequest(url, call, parameters, headers);
            }
            catch (UnAuthorizedException e)
            {
                await RefreshToken();
                return await base.HandleRequest(url, call, parameters, headers);
            }
            
        }

        public async Task<List<PostWrapper>>  GetSaved() => (await MakeRequest<SavedObject>("/user/newnottakenname/saved")).data.children;
    }
}

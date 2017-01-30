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
        private readonly string _user;
        private readonly string _clientId;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientSecret">Get at https://github.com/reddit/reddit/wiki/OAuth2 </param>
        /// <param name="clientId"></param>
        public RedditService(string clientId)
        {
            SetUp("https://www.reddit.com/");
            _clientId = clientId;
        }

        public RedditService(string redditToken, string user)
        {
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

        public async Task<TokenObject> GetAccessToken(string code, string redirectUrl, string clientSecret)
        {
            List<Param> parameters = new List<Param>
            {
                new Param("grant_type","authorization_code"),
                new Param("code", code),
                new Param("redirect_uri", redirectUrl)
            };
            return await MakeRequest<TokenObject>("api/v1/access_token", Call.POST, parameters, new List<Param> { new Param("Authorization", "Basic " + System.Convert.ToBase64String(Encoding.UTF8.GetBytes(_clientId + ":" + clientSecret))) });
        }

        public async Task<List<PostWrapper>>  GetSaved() => (await MakeRequest<SavedObject>("/user/newnottakenname/saved")).data.children;
    }
}

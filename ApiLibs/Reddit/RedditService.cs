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
    /// <summary>
    /// For more explanation see the reddit documentation: https://www.reddit.com/dev/api/
    /// </summary>
    public class RedditService : Service
    {
        private string Refreshtoken { get; }
        private string ClientSecret { get; }
        private readonly string _user;
        private readonly string _clientId;

        /// <summary>
        /// Create the service
        /// </summary>
        /// <param name="redditToken"></param>
        /// <param name="refreshtoken"></param>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <param name="user"></param>
        public RedditService(string redditToken, string refreshtoken, string clientId, string clientSecret, string user)
        {
            Refreshtoken = refreshtoken;
            ClientSecret = clientSecret;
            _clientId = clientId;
            SetUp("https://oauth.reddit.com");
            _user = user;
            AddStandardHeader(new Param("Authorization", "bearer " + redditToken));
        }

        /// <summary>
        /// First connect to ask the user for an access token
        /// </summary>
        /// <param name="oAuth">Your IOAUTH object</param>
        /// <param name="redirectUrl">Where you want the user to be sent to after</param>
        /// <param name="scopes">What scopes you need (see reddit documentation)</param>
        /// <param name="randomString">Add a random string for security reasons</param>
        /// <param name="duration">How long you want the token to be active</param>
        public void Connect(IOAuth oAuth, string redirectUrl, List<string> scopes, string randomString,  string duration = "permanent")
        {
            oAuth.ActivateOAuth(
                new Uri(
                    "https://www.reddit.com/api/v1/authorize?client_id=" + _clientId + "&response_type=code&state=" + randomString + "&redirect_uri=" +
                    redirectUrl + "&duration=" + duration + "&scope=" + scopes.Aggregate((i, j) => i + " " + j)));
        }

        /// <summary>
        /// Call this method to activate your token after you called Connect
        /// </summary>
        /// <param name="code">The code you recieved</param>
        /// <param name="redirectUrl">The redirect url you entered at Connect()</param>
        /// <returns></returns>
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

        /// <summary>
        /// Update your bearer token once it no longer works
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Get all saved posts by your user
        /// </summary>
        /// <returns></returns>
        public async Task<List<PostWrapper>>  GetSaved() => (await MakeRequest<SavedObject>("/user/" + _user + "/saved")).data.children;
    }
}

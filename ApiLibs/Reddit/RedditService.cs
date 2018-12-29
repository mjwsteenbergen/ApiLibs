using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ApiLibs.General;
using ApiLibs.Reddit;
using RestSharp;
using RestSharp.Authenticators;

namespace ApiLibs.Reddit
{
    /// <summary>
    /// For more explanation see the reddit documentation: https://www.reddit.com/dev/api/
    /// </summary>
    public class RedditService : Service
    {
        public UserService UserService => new UserService(this, _user);
        public PostService PostService => new PostService(this, _user);


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
        public RedditService(string refreshtoken, string clientId, string clientSecret, string user) : base("https://oauth.reddit.com")
        {
            Refreshtoken = refreshtoken;
            ClientSecret = clientSecret;
            _clientId = clientId;
            _user = user;
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

        protected internal override async Task<IRestResponse> HandleRequest(string url, Call call = Call.GET, List<Param> parameters = null, List<Param> headers = null, object content = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            try
            {
                return await base.HandleRequest(url, call, parameters, headers, content);
            }
            catch (UnAuthorizedException)
            {
                await RefreshToken();
                return await base.HandleRequest(url, call, parameters, headers, content);
            }
            
        }

    }

    public class RedditScopes
    {
        public static string Account => "account";
        public static string Creddits => "creddits";
        public static string Edit => "edit";
        public static string Flair => "flair";
        public static string History => "history";
        public static string Identity => "identity";
        public static string LiveManage => "livemanage";
        public static string ModConfig => "modconfig";
        public static string ModContributors => "modcontributors";
        public static string ModLog => "modlog";
        public static string ModMail => "modmail";
        public static string ModWiki => "modwiki";
        public static string MySubreddits => nameof(MySubreddits).ToLower();
        public static string PrivateMessages => nameof(PrivateMessages).ToLower();
        public static string Read => "read";
        public static string Report => nameof(Report).ToLower();
        public static string Save => nameof(Save).ToLower();
        public static string StructuredStyles => nameof(StructuredStyles).ToLower();
        public static string Submit => nameof(Submit).ToLower();
        public static string Subscribe => nameof(Subscribe).ToLower();
        public static string Vote => nameof(Vote).ToLower();
        public static string WikiEdit => nameof(WikiEdit).ToLower();
        public static string WikiRead => nameof(WikiRead).ToLower();
    }
}

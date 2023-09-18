using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ApiLibs.GitHub
{
    public class GitHubService : RestSharpService
    {
        internal string GitHub_access_token;
        internal readonly string GitHub_clientID;
        internal readonly string GitHub_client_secret;

        public IssueService IssueService { get; private set; }
        public ActivityService ActivityService { get; private set; }
        public RepositoryService RepositoryService { get; private set; }
        public ActionService ActionService { get; private set; }

        /// <summary>
        /// Use this constructor if you do not have an access token yet
        /// </summary>
        /// <param name="gitHub_clientID"></param>
        /// <param name="gitHub_client_secret"></param>
        public GitHubService(string gitHub_clientID, string gitHub_client_secret): this()
        {
            GitHub_clientID = gitHub_clientID;
            GitHub_client_secret = gitHub_client_secret;
        }

        /// <summary>
        /// Use this constructor if you have gotten an access token
        /// </summary>
        /// <param name="gitHub_access_token"></param>
        public GitHubService(string gitHub_access_token) : this()
        {
            GitHub_access_token = gitHub_access_token ?? throw new ArgumentNullException(nameof(gitHub_access_token));
            AddStandardHeader(new Param("Authorization", "token " + GitHub_access_token));
        }

        public GitHubService() : base("https://api.github.com/")
        {
            IssueService = new IssueService(this);
            RepositoryService = new RepositoryService(this);
            ActivityService = new ActivityService(this);
            ActionService = new ActionService(this);
        }

        public void Connect(IOAuth _authenticator)
        {
            var url = "https://github.com/login/oauth/authorize?redirect_uri=" + _authenticator.RedirectUrl + "&client_id=" + GitHub_clientID + "&scope=repo,notifications,admin:org";
            _authenticator.ActivateOAuth(new Uri(url));
        }

        public async Task<string> ConvertToToken(string returnValue)
        {
            List<Param> parameters = new()
            {
                new Param("client_id", GitHub_clientID),
                new Param("client_secret", GitHub_client_secret),
                new Param("code", returnValue.Replace("code=", ""))
            };

            var content = await new BlandService().MakeRequest<string>("https://github.com/login/oauth/access_token", Call.POST, parameters: parameters);

            Match m = Regex.Match(content, @"{""access_token"":""(\w+)""");
            GitHub_access_token = m.Groups[1].ToString();
            AddStandardHeader(new Param("Authorization", "token " + GitHub_access_token));
            return GitHub_access_token;
        }

        public async Task<GitHubUser> GetUser(string username)
        {
            return await MakeRequest<GitHubUser>("users/" + username);
        }
    }

    
}

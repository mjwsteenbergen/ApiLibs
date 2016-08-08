using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ApiLibs.General;

namespace ApiLibs.GitHub
{
    public class GitHubService : Service
    {
        internal string GitHub_access_token;
        internal readonly string GitHub_clientID;
        internal readonly string GitHub_client_secret;

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
            GitHub_access_token = gitHub_access_token;
        }

        public GitHubService()
        {
            SetUp("https://api.github.com/");
        }

        public async Task<string> Connect(IOAuth _authenticator)
        {
            if(GitHub_access_token == null)
            {
                var url = "https://github.com/login/oauth/authorize?redirect_uri=" + _authenticator.RedirectUrl() + "&client_id=" + GitHub_clientID + "&scope=repo,notifications";
                string key = _authenticator.ActivateOAuth(new Uri(url));
                List<Param> parameters = new List<Param>
                {
                    new Param("client_id", GitHub_clientID),
                    new Param("client_secret", GitHub_client_secret),
                    new Param("code", key.Replace("code=", ""))
                };

                SetBaseUrl("https://github.com/");
                IRestResponse resp = await HandleRequest("login/oauth/access_token", Call.POST, parameters: parameters);

                Match m = Regex.Match(resp.Content, @"{""access_token"":""(\w+)""");
                GitHub_access_token = m.Groups[1].ToString();
            }

            AddStandardHeader(new Param("Authorization", "token " + GitHub_access_token));
            SetBaseUrl("https://api.github.com/");
            return GitHub_access_token;
        }

        public async Task<List<Issue>> GetPullRequests(Repository repo)
        {
            List<Issue> res = new List<Issue>();
            foreach (Issue issue in await GetIssuesAndPRs(repo))
            {
                if (issue.pull_request != null)
                {
                    res.Add(issue);
                }
            }
            return res;
        }

        public async Task<List<Issue>> GetIssues(Repository repo)
        {
            List<Issue> res = new List<Issue>();
            foreach (Issue issue in await GetIssuesAndPRs(repo))
            {
                if (issue.pull_request == null)
                {
                    res.Add(issue);
                }
            }
            return res;
        }

        public async Task<List<Issue>> GetIssuesAndPRs(Repository repo)
        {
            return await MakeRequest<List<Issue>>(repo.issues_url);
        }

        public async Task<GitHubUser> GetUser(string username)
        {
            return await MakeRequest<GitHubUser>("users/" + username);
        }

        public async Task<List<Repository>> GetMyRepositories()
        {
            return await MakeRequest<List<Repository>>("user/repos");
        }

        public async Task<Repository> GetRepository(string user, string repository)
        {
            return await MakeRequest<Repository>("repos/" + user + "/" + repository);
        }

        public async Task<Repository> GetRepository(string name)
        {
            foreach(Repository repo in await GetMyRepositories())
            {
                if(repo.name == name)
                {
                    return repo;
                }
            }
            throw new KeyNotFoundException("Could not find" + name);
        }

        public async Task<Issue> AddIssue(OpenIssue issue, Repository repo)
        {   
            return await MakeRequest<Issue>(repo.issues_url, Call.POST, new List<Param>(), content: issue);
        }

        

        public async Task<List<NotificationsObject>> GetNotifications()
        {
            return await MakeRequest<List<NotificationsObject>>("notifications", Call.GET);
        }

        public async Task<Issue> CloseIssue(Issue it)
        {
            ModifyIssue issue = it.ConvertToRequest();
            issue.state = "closed";
            return await ModifyIssue(it.url, issue);
        }

        public async Task<Issue> ModifyIssue(string issueUrl, ModifyIssue it)
        {
            return await MakeRequest<Issue>(issueUrl, Call.PATCH, content: it);
        }

        public async Task<List<Release>> GetReleases(string owner, string repo)
        {
            return (await MakeRequest<Release[]>("repos/" + owner + "/" + repo + "/releases")).ToList();
        }

        /// <summary>
        /// WARNING: DOES NOT WORK
        /// </summary>
        /// <param name="notification"></param>
        public void MarkNotificationRead(NotificationsObject notification)
        {
            //await MakeRequest("")
        }

        /// <summary>
        /// WARNING: DOES NOT WORK
        /// </summary>
        /// <returns></returns>
        public async Task MarkNotificationsReadRepo(NotificationsObject notification)
        {
            await MakeRequest<NotificationsObject>(notification.repository.notifications_url, Call.PUT, new List<Param>());
        }

        /// <summary>
        /// WARNING: DOES NOT WORK
        /// </summary>
        /// <returns></returns>
        public async Task MarkNotificationsRead(NotificationsObject notification)
        {
            await MakeRequest<NotificationsObject>(notification.url, Call.PATCH, new List<Param>());
        }
    }

    
}

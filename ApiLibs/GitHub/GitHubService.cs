using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ApiLibs.GitHub
{
    public class GitHubService : Service
    {
        private IOAuth authenticator;

        public GitHubService(IOAuth authenticator)
        {
            SetUp("https://github.com/");
            this.authenticator = authenticator;
            //AddStandardParameter(new Param("user-agent", "Zeus"));
        }


        public async Task Connect()
        {
            if(Passwords.GitHub_access_token == null)
            {
                string key = authenticator.ActivateOAuth(new Uri("https://github.com/login/oauth/authorize?redirect_uri=" + authenticator.redirectUrl() + "&client_id=" + Passwords.GitHub_clientID + "&scope=repo,notifications"));
                List<Param> parameters = new List<Param>();
                parameters.Add(new Param("client_id", Passwords.GitHub_clientID));
                parameters.Add(new Param("client_secret", Passwords.GitHub_client_secret));
                parameters.Add(new Param("code", key.Replace("code=", "")));

                IRestResponse resp = await MakeRequestPost("login/oauth/access_token", parameters);

                Match m = Regex.Match(resp.Content, @"{""access_token"":""(\w+)""");
                Passwords.addPassword("GitHub_access_token", m.Groups[1].ToString());
                Passwords.writePasswords();
            }

            AddStandardHeader(new Param("Authorization", "token " + Passwords.GitHub_access_token));
            setBaseUrl("https://api.github.com/");
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
            return await MakeRequest<List<Issue>>(repo.issues_url, new List<Param>());
        }

        public async Task<GitHubUser> GetUser(string username)
        {
            return await MakeRequest<GitHubUser>("users/newnottakenname", new List<Param>());
        }

        public async Task<List<Repository>> GetMyRepositories()
        {
            return await MakeRequest<List<Repository>>("user/repos", new List<Param>());
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
            return await MakeRequestPost<Issue>(repo.issues_url, new List<Param>(), issue);
        }

        

        public async Task<List<NotificationsObject>> GetNotifications()
        {
            return await MakeRequest<List<NotificationsObject>>("notifications", new List<Param>());
        }

        public async Task<Issue> CloseIssue(Issue it)
        {
            ModifyIssue issue = it.ConvertToRequest();
            issue.state = "closed";
            return await ModifyIssue(it.url, issue);
        }

        public async Task<Issue> ModifyIssue(string issueUrl, ModifyIssue it)
        {
            return await MakeRequestPatch<Issue>(issueUrl, it);
        }

        
    }

    
}

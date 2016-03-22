﻿using Newtonsoft.Json;
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
        private readonly IOAuth _authenticator;
        internal string GitHub_access_token;
        internal readonly string GitHub_clientID;
        internal readonly string GitHub_client_secret;

        public GitHubService(IOAuth authenticator, Passwords pass)
        {
            SetUp("https://github.com/");
            this._authenticator = authenticator;
            GitHub_access_token = pass.GitHub_access_token;
            GitHub_clientID = pass.GitHub_clientID;
            GitHub_client_secret = pass.GitHub_client_secret;
        }

        public async Task Connect(Passwords password)
        {
            password.AddPassword("GitHub_access_token", await Connect());
        }

        public async Task<string> Connect()
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

                IRestResponse resp = await MakeRequest("login/oauth/access_token", Call.POST, parameters);

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
            return await MakeRequest<List<Issue>>(repo.issues_url, new List<Param>());
        }

        public async Task<GitHubUser> GetUser(string username)
        {
            return await MakeRequest<GitHubUser>("users/" + username, new List<Param>());
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
            return await MakeRequest<Issue>(repo.issues_url, Call.POST, new List<Param>(), issue);
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
            return await MakeRequest<Issue>(issueUrl, Call.PATCH, it);
        }

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

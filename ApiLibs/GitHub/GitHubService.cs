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

            AddStandardParameter(new Param("content-type", "application/json"));
            AddStandardParameter(new Param("content-length", "37"));
            AddStandardParameter(new Param("user-agent", "Zeus"));
        }


        public async Task Connect()
        {
            if(Passwords.GitHub_access_token == null)
            {
                string key = await authenticator.ActivateOAuth(new Uri("https://github.com/login/oauth/authorize?redirect_uri=" + authenticator.redirectUrl() + "&client_id=" + Passwords.GitHub_clientID + "&scope=repo,notifications"));
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

        public async Task<GitHubUser> GetUser(string username)
        {
            List<Param> parameters = new List<Param>();
            return await Convert<GitHubUser>(await MakeRequest("users/newnottakenname", parameters));
        }

        public async Task<List<Repository>> GetMyRepository()
        {
            return await Convert<List<Repository>>(await MakeRequest("user/repos", new List<Param>()));
        }

        public async Task<List<NotificationsObject>> GetNotifications()
        {
            return await Convert<List<NotificationsObject>>(await MakeRequest("notifications", new List<Param>()));
        }

    }
}

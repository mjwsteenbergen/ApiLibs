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
            this.authenticator = authenticator;
            Passwords.readPasswords();

            AddStandardParameter(new Param("content-type", "application/json"));
            AddStandardParameter(new Param("content-length", "37"));
            AddStandardParameter(new Param("user-agent", "Zeus"));
        }


        public async Task Connect()
        {
            SetUp("https://github.com/");
            string key = await authenticator.ActivateOAuth(new Uri("https://github.com/login/oauth/authorize?redirect_uri=" + authenticator.redirectUrl() + "&client_id=" + Passwords.GitHub_clientID + "&scope=repo,notifications"));
            List<Param> parameters = new List<Param>();
            parameters.Add(new Param("client_id", Passwords.GitHub_clientID));
            parameters.Add(new Param("client_secret", Passwords.GitHub_client_secret));
            parameters.Add(new Param("code", key.Replace("code=","")));

            IRestResponse resp = await MakeRequestPost("login/oauth/access_token", parameters);

            if (resp.StatusCode.ToString() != "OK")
            {
                foreach (Parameter p in resp.Headers)
                {
                    Console.WriteLine(p.ToString());
                }

                throw new Exception("\n" + "A problem occured in PocketService.Connect() - " +
                                    resp.StatusCode.ToString() + "\n" + resp.Content);
            }
            Match m = Regex.Match(resp.Content, @"{""access_token"":""(\w+)""");
            Passwords.addPassword("GitHub_access_token", m.Groups[1].ToString());
            Passwords.writePasswords();
            setBaseUrl("https://api.github.com/");
        }

        public async Task<List<NotificationsObject>> GetNotifications()
        {
            IRestResponse resp = await MakeRequest("notifications", new List<Param>());
            List<NotificationsObject> r = JsonConvert.DeserializeObject<List<NotificationsObject>>(resp.Content);
            return r;
        }

    }
}

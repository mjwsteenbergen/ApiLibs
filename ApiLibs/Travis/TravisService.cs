using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiLibs.GitHub;

namespace ApiLibs.Travis
{
    public class TravisService : Service
    {
        private readonly IOAuth _authenticator;
        private string Travis_Token;

        public TravisService(IOAuth authenticator, Passwords pass)
        {
            _authenticator = authenticator;
            Travis_Token = pass.Travis_Token;
        }

        public async Task Connect(IOAuth authenticator, Passwords pass)
        {
            if (pass.GitHub_access_token == null)
            {
                GitHubService g_serv = new GitHubService(authenticator, pass);
                await g_serv.Connect(pass);
            }
            pass.AddPassword("Travis_Token", await Connect(pass.GitHub_access_token));
        }

        public async Task<string> Connect(string GitHub_access_token)
        {
            SetUp("https://api.travis-ci.org");
            Client.UserAgent = "Travis";
            AddStandardHeader(new Param("Accept", "application/vnd.travis-ci.2+json"));
            AddStandardHeader(new Param("User-Agent", "Travis"));
            if (Travis_Token == null)
            {
                Travis_Token = (await MakeRequest<Auth>("/auth/github", Call.POST, new List<Param> { new Param("github_token", GitHub_access_token) })).access_token;
            }

            AddStandardHeader(new Param("Authorization", "token" + Travis_Token));

            return Travis_Token;
        }

        public async Task<BuildList> GetLatestBuilds(string repository)
        {
            return await MakeRequest<BuildList>(repository, new List<Param>());
        }
    }
}

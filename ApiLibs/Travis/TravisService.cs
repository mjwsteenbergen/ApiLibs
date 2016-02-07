using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiLibs.General;
using ApiLibs.GitHub;

namespace ApiLibs.Travis
{
    public class TravisService : Service, IService
    {
        private readonly IOAuth _authenticator;

        public TravisService(IOAuth authenticator)
        {
            _authenticator = authenticator;
            SetUp("https://api.travis-ci.org");
            Client.UserAgent = "Travis";
        }

        public async Task Connect()
        {
            await MakeRequest("/", Call.GET, new List<Param>());
            AddStandardHeader(new Param("Accept", "application/vnd.travis-ci.2+json"));
            AddStandardHeader(new Param("User-Agent", "Travis"));
            if (Passwords.Travis_Token == null)
            {
                GitHubService github = new GitHubService(_authenticator);
                await github.Connect();

                Passwords.AddPassword("Travis_Token", (await MakeRequest<Auth>("/auth/github", Call.POST, new List<Param> { new Param("github_token", Passwords.GitHub_access_token) })).access_token);
            }

            AddStandardHeader(new Param("Authorization", "token" + Passwords.Travis_Token));

        }

        public async Task<BuildList> GetLatestBuilds(string repository)
        {
            return await MakeRequest<BuildList>(repository, new List<Param>());
        }
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiLibs.Travis
{
    public class TravisService : RestSharpService
    {
        private string Travis_Token;

        /// <summary>
        /// WARNING: Only to be used when you don't have an access token and are about to call Connect.
        /// </summary>
        public TravisService() : base("https://api.travis-ci.org")
        {
            (Implementation as RestSharpImplementation).Client = new RestSharp.RestClient(new RestSharp.RestClientOptions
            {
                UserAgent = "Travis"
            });
        }

        public TravisService(string travis_Token): base("https://api.travis-ci.org")
        {
            Travis_Token = travis_Token;
            AddStandardHeader(new Param("Accept", "application/vnd.travis-ci.2+json"));
            AddStandardHeader(new Param("User-Agent", "Travis"));
            AddStandardHeader(new Param("Authorization", "token" + Travis_Token));
        }

        public async Task<string> Connect(string GitHub_access_token)
        {
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
            return await MakeRequest<BuildList>(repository);
        }
    }
}

using ApiLibs.General;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApiLibs.GitHub
{
    public class RepositoryService : SubService<GitHubService>
    {
        public RepositoryService(GitHubService service) : base(service)
        {
        }

        public async Task<List<Repository>> GetUserRepositories(Visibility? vis = null, Affiliation? aff = null)
        {
            return await MakeRequest<List<Repository>>("user/repos", parameters: new List<Param>
            {
                new OParam("visibility", vis?.ToString()),
                new OParam("affiliation", aff?.ToString()),
                new Param("per_page", "100")
            });
        }

        public enum Visibility { all, @public, @private }
        public enum Affiliation { owner, collaborator, organisation_member }

        public async Task<Repository> GetRepository(string user, string repository)
        {
            return await MakeRequest<Repository>("repos/" + user + "/" + repository);
        }

        public async Task<Repository> GetRepository(Uri repositoryUri)
        {
            return await MakeRequest<Repository>(repositoryUri.AbsolutePath);
        }

        public async Task<Repository> GetRepository(string name)
        {
            var repositories = await GetUserRepositories();
            foreach (Repository repo in repositories)
            {
                if (repo.Name == name)
                {
                    return repo;
                }
            }
            throw new KeyNotFoundException("Could not find" + name);
        }

        /// <summary>
        /// Returns the specific release
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="repo"></param>
        /// <param name="id"></param>
        public async Task<Release> GetRelease(string owner, string repo, string id)
        {
            return await MakeRequest<Release>("repos/" + owner + "/" + repo + "/releases/tags/" + id);
        }

        public async Task<Release> GetRelease(string owner, string repo, int id)
        {
            return await MakeRequest<Release>("repos/" + owner + "/" + repo + "/releases/" + id);
        }

        public async Task<List<Release>> GetReleases(string owner, string repo)
        {
            return (await MakeRequest<List<Release>>("repos/" + owner + "/" + repo + "/releases"));
        }
    }
}

using ApiLibs.General;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace ApiLibs.GitHub
{
    public class ActionService : SubService<GitHubService>
    {
        public ActionService(GitHubService service) : base(service)
        {
        }

        public Task CreateWorkflowDispatchEvent(Repository repo, string workflow_id, string branch, object inputs = null)
        {
            return CreateWorkflowDispatchEvent(repo.Owner.Login, repo.Name, workflow_id, branch, inputs);
        }

        /// <summary>
        /// You can use this endpoint to manually trigger a GitHub Actions workflow run. You can replace workflow_id with the workflow file name. For example, you could use main.yaml.
        /// </summary>
        /// <returns></returns>
        public Task CreateWorkflowDispatchEvent(string owner, string repo, string workflow_id, string branch, object inputs = null)
        {
            return MakeRequest($"/repos/{owner}/{repo}/actions/workflows/{workflow_id}/dispatches", Call.POST, content: new DispatchContent {
                Reference = branch,
                Inputs = inputs
            }, statusCode: System.Net.HttpStatusCode.NoContent);
        }

        class DispatchContent {
            [JsonProperty("ref")]
            public string Reference { get; set; }

            [JsonProperty("inputs")]
            public object Inputs { get; set; }
        }
    }
}

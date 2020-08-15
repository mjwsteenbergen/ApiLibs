using ApiLibs.General;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace ApiLibs.GitHub
{
    public class IssueService : SubService
    {
        public IssueService(GitHubService service) : base(service)
        {
        }

        public async Task<List<Issue>> GetIssues()
        {
            return await MakeRequest<List<Issue>>("user/issues", parameters: new List<Param> { new Param("per_page", 100) });
        }

        public async Task<List<Issue>> GetIssues(Repository repo)
        {
            return (await GetIssuesAndPRs(repo)).Where(i => i.PullRequest == null).ToList();
        }

        private string IssueUrl(Repository repo)
        {
            return repo.IssuesUrl.Replace("https://api.github.com/", "").Replace("{/number}", "");
        }

        public async Task<List<Issue>> GetIssuesAndPRs(Repository repo)
        {
            return await MakeRequest<List<Issue>>(IssueUrl(repo));
        }

        public async Task<Issue> AddIssue(OpenIssue issue, Repository repo)
        {
            return await MakeRequest<Issue>(IssueUrl(repo), Call.POST, new List<Param>(), content: issue);
        }

        public async Task<Issue> CloseIssue(Issue it)
        {
            ModifyIssue issue = it.ConvertToRequest();
            issue.state = "closed";
            return await ModifyIssue(it.Url, issue);
        }

        public async Task<Issue> ModifyIssue(Uri issueUrl, ModifyIssue it)
        {
            return await MakeRequest<Issue>(issueUrl.AbsolutePath, Call.PATCH, content: it);
        }

        public async Task<Issue> GetIssue(string user, string repo, int issueNumber)
        {
            return await MakeRequest<Issue>("/repos/" + user + "/" + repo + "/issues/" + issueNumber);
        }

        public Task<Comment> AddComment(Issue issue, string body) => AddComment(issue.RepositoryOwner, issue.RepositoryName, issue.Number.ToString(), body);

        public Task<Comment> AddComment(string owner, string repo, string issue_number, string body) => MakeRequest<Comment>($"/repos/{owner}/{repo}/issues/{issue_number}/comments", Call.POST, content: new {
            body = body
        });

        public async Task<List<Issue>> GetPullRequests(Repository repo)
        {
            List<Issue> res = new List<Issue>();
            foreach (Issue issue in await GetIssuesAndPRs(repo))
            {
                if (issue.PullRequest != null)
                {
                    res.Add(issue);
                }
            }
            return res;
        }
    }
}

using System.Threading.Tasks;
using ApiLibs.General;

namespace ApiLibs.Reddit
{
    public class SubredditService : SubService
    {
        private string _user;

        public SubredditService(RedditService redditService, string user) : base(redditService)
        {
            _user = user;
        }

        public async Task<SubredditResult> GetPosts(string subreddit, string order = "hot", int limit = 100)
        {
            return await MakeRequest<SubredditResult>($"/r/{subreddit}/{order}?limit={limit}");
        }
    }
}
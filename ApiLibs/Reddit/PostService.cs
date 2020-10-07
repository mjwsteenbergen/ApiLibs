using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiLibs.General;

namespace ApiLibs.Reddit
{
    public class PostService : SubService<RedditService>
    {
        private string _user;

        public PostService(RedditService redditService, string user) : base(redditService)
        {
            _user = user;
        }

        public async Task<IEnumerable<RedditPost>> GetPosts(string subreddit, string order = "hot", int limit = 10)
        {
            return (await MakeRequest<SubredditResult>($"/r/{subreddit}/{order}?limit={limit}")).Data.Children.Select(i => i.Data);
        }

        public async Task<IEnumerable<ChildData>> GetComments(string subreddit, string postId, string order = "top", int limit = 10)
        {
            var commentsResults = await MakeRequest<CommentsResult[]>($"/r/{subreddit}/comments/{postId}?sort={order}&limit={limit}");
            return commentsResults[1].Data.Children.Select(j => j.Data);
        }

        public async Task<IEnumerable<ChildData>> GetComments(RedditPost post, string order = "top", int limit = 10)
        {
            return await GetComments(post.Subreddit, post.Id, order, limit);
        }
    }
}
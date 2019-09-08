using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiLibs.General;
using ApiLibs.Reddit;
using NUnit.Framework;

namespace ApiLibsTest.Reddit
{
    class RedditTest
    {
        private RedditService reddit;

        [SetUp]
        public async Task Setup()
        {
            Passwords passwords = await Passwords.ReadPasswords();
            reddit = new RedditService(passwords.RedditRefreshToken, passwords.RedditClient,
                passwords.RedditSecret, passwords.RedditUser);
        }

        [Test]
        public async Task ConnectTest()
        {
            Passwords passwords = await Passwords.ReadPasswords();
            reddit.Connect(new StupidOAuth(), passwords.GeneralRedirectUrl, new List<string>
            {
                RedditScopes.History,
                RedditScopes.Read,
                RedditScopes.MySubreddits,
                RedditScopes.Save
            }, "blergh");
        }

        [Test]
        public async Task ConvertToTokenTest()
        {
            string token = "";
            Passwords passwords = await Passwords.ReadPasswords();
            var refreshtoken = await reddit.GetAccessToken(token, passwords.GeneralRedirectUrl);
        }

        [Test]
        public async Task TestSaved()
        {
            Assert.NotNull(await reddit.UserService.GetSaved());
        }

        [Test]
        public async Task TestPosts()
        {
            var res = await reddit.PostService.GetPosts("cortex", "new", 10);
        }

        [Test]
        public async Task TestComments()
        {
            var post = await reddit.PostService.GetPosts("cortex", "new", 10);
            var res = await reddit.PostService.GetComments(post.First());
            var s = res.First().CreatedUtc;
        }
    }
}

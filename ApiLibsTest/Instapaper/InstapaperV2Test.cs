using System;
using System.Linq;
using System.Threading.Tasks;
using ApiLibs.General;
using ApiLibs.Instapaper.V2;
using NUnit.Framework;

namespace ApiLibsTest.Instapaper
{
    [Explicit]
    class InstapaperV2Test
    {
        InstapaperV2Service instapaper;

        [SetUp]
        public async Task Setup()
        {
            Passwords passwords = await Passwords.ReadPasswords();
            instapaper = new InstapaperV2Service(passwords.InstapaperAccessToken);
        }

        [Test]
        public async Task TestGetMe()
        {
            var me = await instapaper.GetMe();
            Assert.That(me.Id, Is.GreaterThan(0));
        }

        [Test]
        public async Task TestGetBookmarks()
        {
            var res = await instapaper.Bookmarks.GetBookmarks();
            Assert.That(res.Bookmarks, Is.Not.Null);
        }

        [Test]
        public async Task TestGetBookmarksArchive()
        {
            var res = await instapaper.Bookmarks.GetBookmarks(InstapaperBookmarkSection.Archive, limit: 200);
            Assert.That(res.Bookmarks, Is.Not.Null);
        }

        [Test]
        public async Task TestSync()
        {
            var res = await instapaper.Bookmarks.Sync(DateTimeOffset.UtcNow.AddDays(-7));
            Assert.That(res.DeletedIds, Is.Not.Null);
        }

        [Test]
        public async Task TestAddAndDeleteBookmark()
        {
            var bookmark = await instapaper.Bookmarks.AddBookmark("https://example.com/", "Example", tags: new[] { "ApiLibsTest" });
            await instapaper.Bookmarks.LikeBookmark(bookmark);
            await instapaper.Bookmarks.UpdateReadProgress(bookmark, 0.5);
            await instapaper.Bookmarks.ArchiveBookmark(bookmark);
            await instapaper.Bookmarks.DeleteBookmark(bookmark);
        }

        [Test]
        public async Task TestGetFoldersAndTags()
        {
            await instapaper.Folders.GetFolders();
            await instapaper.Tags.GetTags();
        }

        [Test]
        public async Task TestGetParsedArticle()
        {
            var bookmark = (await instapaper.Bookmarks.GetBookmarks(limit: 1)).Bookmarks.First();
            var article = await instapaper.Bookmarks.GetParsedArticle(bookmark);
            Assert.That(article.Content, Is.Not.Null);
        }
    }
}

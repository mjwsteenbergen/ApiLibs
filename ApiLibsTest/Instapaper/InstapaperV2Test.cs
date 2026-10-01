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
            instapaper = new InstapaperV2Service(passwords.Instaper_access_token);
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
            var res = await instapaper.BookmarkService.GetBookmarks();
            Assert.That(res.Bookmarks, Is.Not.Null);
        }

        [Test]
        public async Task TestGetBookmarksArchive()
        {
            var res = await instapaper.BookmarkService.GetBookmarks(InstapaperBookmarkSection.Archive, limit: 200);
            Assert.That(res.Bookmarks, Is.Not.Null);
        }

        [Test]
        public async Task TestSync()
        {
            var res = await instapaper.BookmarkService.Sync(DateTimeOffset.UtcNow.AddDays(-7));
            Assert.That(res.DeletedIds, Is.Not.Null);
        }

        [Test]
        public async Task TestAddAndDeleteBookmark()
        {
            var bookmark = await instapaper.BookmarkService.AddBookmark("https://example.com/", "Example", tags: new[] { "ApiLibsTest" });
            await instapaper.BookmarkService.LikeBookmark(bookmark);
            await instapaper.BookmarkService.UpdateReadProgress(bookmark, 0.5);
            await instapaper.BookmarkService.ArchiveBookmark(bookmark);
            await instapaper.BookmarkService.DeleteBookmark(bookmark);
        }

        [Test]
        public async Task TestGetFoldersAndTags()
        {
            await instapaper.FolderService.GetFolders();
            await instapaper.TagService.GetTags();
        }

        [Test]
        public async Task TestGetParsedArticle()
        {
            var bookmark = (await instapaper.BookmarkService.GetBookmarks(limit: 1)).Bookmarks.First();
            var article = await instapaper.BookmarkService.GetParsedArticle(bookmark);
            Assert.That(article.Content, Is.Not.Null);
        }
    }
}

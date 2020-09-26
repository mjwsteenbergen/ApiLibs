using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ApiLibs.General;
using ApiLibs.Instapaper;
using ApiLibs.Pocket;
using NUnit.Framework;

namespace ApiLibsTest.Instapaper
{
    class InstapaperTest
    {
        InstapaperService instapaper;

        [SetUp]
        public async Task Setup()
        {
            Passwords passwords = await Passwords.ReadPasswords();
            instapaper = new InstapaperService(passwords.Instaper_ID, passwords.Instaper_secret,
                passwords.Instaper_user_token, passwords.Instaper_user_secret);
        }

        [Test]
        [Ignore("Overrides some stuff")]
        public async Task ConnectTest()
        {
            Passwords passwords = await Passwords.ReadPasswords();
            var insta = new InstapaperService();
            var res = insta.Connect("username", "password", passwords.Instaper_ID, passwords.Instaper_secret);
            passwords.Instaper_user_token = res.token;
            passwords.Instaper_user_secret = res.secret;
            await passwords.WriteToFile();
        }

        [Test]
        public async Task TestGetBookmarks()
        {
            var res = await instapaper.GetBookmarks();
        }

        [Test]
        public async Task TestGetBookmarksLimit()
        {
            var res = await instapaper.GetBookmarks(limit:200);
        }

        [Test]
        public async Task TestGetBookmarksStarred()
        {
            var res = await instapaper.GetBookmarks("starred");
        }

        [Test]
        public async Task TestGetBookmarksArchive()
        {
            var res = await instapaper.GetBookmarks("archive");
        }

        [Test]
        public async Task TestGetAllBookmarkInfo()
        {
            var res = await instapaper.GetAllBookmarkInfo();
        }
    }
}

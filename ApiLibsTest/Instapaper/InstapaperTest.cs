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
        public void Setup()
        {
            Passwords passwords = Passwords.ReadPasswords();
            instapaper = new InstapaperService(passwords.Instaper_ID, passwords.Instaper_secret,
                passwords.Instaper_user_token, passwords.Instaper_user_secret);
        }

        [Test]
        [Ignore("Overrides some stuff")]
        public async Task ConnectTest()
        {
            Passwords passwords = Passwords.ReadPasswords();
            var insta = new InstapaperService();
            var res = insta.Connect("username", "password", passwords.Instaper_ID, passwords.Instaper_secret);
            passwords.Instaper_user_token = res.token;
            passwords.Instaper_user_secret = res.secret;
            passwords.WriteToFile();
        }

        [Test]
        public async Task TestActionAttribute()
        {
            var res = await instapaper.GetBookmarks();
        }
    }
}

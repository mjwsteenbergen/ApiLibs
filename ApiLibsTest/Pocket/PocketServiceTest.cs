using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ApiLibs.General;
using ApiLibs.Pocket;
using ApiLibs.Spotify;
using NUnit.Framework;

namespace ApiLibsTest
{
    class PocketServiceTest
    {
        PocketService pocket;

        [SetUp]
        public async Task Setup()
        {
            Passwords passwords = await Passwords.ReadPasswords();
            pocket = new PocketService(passwords.Pocket_access_token, passwords.PocketKey);
        }

        [Test]
        [Ignore("Startup")]
        public async Task ConnectTest()
        {
            Passwords passwords = await Passwords.ReadPasswords();
            var pocket = new PocketService(passwords.PocketKey);
            var code = await pocket.Connect(new StupidOAuth(), "http://nntn.nl");
        }

        [Test]
        [Ignore("Startup")]
        public async Task GetToken()
        {
            Passwords passwords = await Passwords.ReadPasswords();
            var pocket = new PocketService(passwords.PocketKey);
            var accessToken = await pocket.ConvertToToken("");
//            pocket.Connect()
        }

        [Test]
        public async Task GetBookmarks()
        {
            var res = await pocket.GetReadingList();
        }
    }
}

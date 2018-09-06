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
        public void Setup()
        {
            Passwords passwords = Passwords.ReadPasswords();
            pocket = new PocketService(passwords.Pocket_access_token, passwords.PocketKey);
        }

        [Test]
        public async Task ConnectTest()
        {
            Passwords passwords = Passwords.ReadPasswords();
            var pocket = new PocketService(passwords.PocketKey);
            var code = await pocket.Connect(new StupidOAuth(), "http://nntn.nl");
        }

        [Test]
        public async Task GetToken()
        {
            Passwords passwords = Passwords.ReadPasswords();
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

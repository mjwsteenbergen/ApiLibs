using ApiLibs.General;
using ApiLibs.Trakt;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApiLibsTest.Trakt
{
    [Explicit]
    class TraktTest
    {
        public static async Task<TraktService> GetTrakt()
        {
            Passwords pass = await Passwords.ReadPasswords();
            return new TraktService(pass.TraktAccessToken, pass.TraktRefreshToken, pass.TraktId, pass.TraktSecret, "https://nntn.nl");
        }

        [Test]
        [Ignore("Connect")]
        public async Task ConnectTest()
        {
            Passwords pass = await Passwords.ReadPasswords();
            var id = pass.TraktId;
            var secret = pass.TraktSecret;
            TraktService trakt = new TraktService(id, "", "https://nntn.nl");
            trakt.Connect(new StupidOAuth());
        }

        [Test]
        [Ignore("Connect")]
        public async Task ConvertToToken()
        {
            Passwords pass = await Passwords.ReadPasswords();
            var id = pass.TraktId;
            var secret = pass.TraktSecret;
            TraktService trakt = new TraktService(id, secret, pass.GeneralRedirectUrl);
            var token = await trakt.ConvertToToken("YOUR TOKEN");
        }


    }
}

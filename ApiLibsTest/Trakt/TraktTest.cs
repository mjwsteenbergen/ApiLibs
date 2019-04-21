using ApiLibs.General;
using ApiLibs.Trakt;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApiLibsTest.Trakt
{
    class TraktTest
    {
        public static TraktService GetTrakt()
        {
            Passwords pass = Passwords.ReadPasswords();
            return new TraktService(pass.TraktAccessToken, pass.TraktRefreshToken, pass.TraktId, pass.TraktSecret, "https://nntn.nl");
        }

        [Test]
        [Ignore("Connect")]
        public void ConnectTest()
        {
            Passwords pass = Passwords.ReadPasswords();
            var id = pass.TraktId;
            var secret = pass.TraktSecret;
            TraktService trakt = new TraktService();
            trakt.Connect(new StupidOAuth(), id, "https://nntn.nl");
        }

        [Test]
        [Ignore("Connect")]
        public async Task ConvertToToken()
        {
            Passwords pass = Passwords.ReadPasswords();
            var id = pass.TraktId;
            var secret = pass.TraktSecret;
            TraktService trakt = new TraktService();
            var token = await trakt.ConvertToToken("YOUR TOKEN", id, secret, pass.GeneralRedirectUrl);
        }


    }
}

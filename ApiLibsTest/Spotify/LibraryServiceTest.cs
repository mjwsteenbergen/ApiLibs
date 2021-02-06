using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ApiLibs.General;
using ApiLibs.Spotify;
using NUnit.Framework;

namespace ApiLibsTest.Spotify
{
    class LibraryServiceTest
    {
        private LibraryService libraryService;
        private SpotifyService spotify;

        [OneTimeSetUp]
        public async Task SetUp()
        {
            Passwords passwords = await Passwords.ReadPasswords();
            spotify = new SpotifyService(passwords.SpotifyRefreshToken, passwords.SpotifyClientId, passwords.SpotifySecret);
            libraryService = spotify.LibraryService;
        }

        [Test]
        public async Task GetMyTracksTest()
        {
            Assert.NotNull(await libraryService.GetMySavedTracks());
        }
    }
}

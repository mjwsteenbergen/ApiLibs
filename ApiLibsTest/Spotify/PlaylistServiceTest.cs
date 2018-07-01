using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiLibs.General;
using ApiLibs.Spotify;
using NUnit.Framework;

namespace ApiLibsTest.Spotify
{
    class PlaylistServiceTest
    {
        private PlaylistService playlistService;
        private SpotifyService spotify;

        [OneTimeSetUp]
        public void SetUp()
        {
            Passwords passwords = Passwords.ReadPasswords();
            spotify = new SpotifyService(passwords.SpotifyRefreshToken, passwords.SpotifyClientId, passwords.SpotifySecret);
            playlistService = spotify.PlaylistService;
        }

        [Test]
        public async Task GetMyPlaylistsTest()
        {
            Assert.Greater((await playlistService.GetMyPlaylists()).Items.Count, 0);
        }

        [Test]
        [Ignore("Modifies State")]
        public async Task DeletePlaylistTest()
        {
            await playlistService.DeletePlaylist("", "");
        }
    }
}

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
    class SpotifyTest
    {
        private SpotifyService spotify;

        [OneTimeSetUp]
        public void SetUp()
        {
            Passwords passwords = Passwords.ReadPasswords(Memory.ApplicationPath + "Laurentia" + Path.DirectorySeparatorChar);
            spotify = new SpotifyService(passwords.SpotifyRefreshToken, passwords.SpotifyClientId, passwords.SpotifySecret);
        }

        #region auth 

        [Test]
        [Ignore("Modifies State")]
        public async Task Login()
        {
            Passwords passwords = Passwords.ReadPasswords(Memory.ApplicationPath + "Laurentia" + Path.DirectorySeparatorChar);
            await spotify.Connect(new StupidOAuth(),
            passwords.
            );
            Assert.IsNotEmpty(result.tracks.items);
        }


        #endregion
        #region Search

        [Test]
        public async Task SearchTrackTest()
        {
            var result = await spotify.Search("Never gonna give you up", SpotifyService.SearchType.Track);
            Assert.IsNotEmpty(result.tracks.items);
        }

        [Test]
        public async Task SearchArtistTest()
        {
            var result = await spotify.Search("Rick Astley", SpotifyService.SearchType.Artist);
            Assert.IsNotEmpty(result.artists.items);
        }

        [Test]
        public async Task SearchAlbumTest()
        {
            var result = await spotify.Search("Whenever you need somebody", SpotifyService.SearchType.Album);
            Assert.IsNotEmpty(result.albums.items);
        }

        [Test]
        public async Task SearchPlayListTest()
        {
            var result = await spotify.Search("Rick Astley: Top Tracks", SpotifyService.SearchType.Playlist);
            Assert.IsNotEmpty(result.playlists.items);
        }
    #endregion
    }
}

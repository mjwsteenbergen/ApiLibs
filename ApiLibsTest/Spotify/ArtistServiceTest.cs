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
    class ArtistServiceTest
    {
        private ArtistService artistService;
        private SpotifyService spotify;

        [OneTimeSetUp]
        public void SetUp()
        {
            Passwords passwords = Passwords.ReadPasswords();
            spotify = new SpotifyService(passwords.SpotifyRefreshToken, passwords.SpotifyClientId, passwords.SpotifySecret);
            artistService = spotify.ArtistService;
        }

        [Test]
        public async Task GetArtistTest()
        {
            Assert.NotNull(await artistService.GetArtist("6MDME20pz9RveH9rEXvrOM"));
        }

        [Test]
        public async Task GetArtistsTest()
        {
            Assert.NotNull(await artistService.GetArtists(new List<string>
            {
                "6MDME20pz9RveH9rEXvrOM",
                "1Xylc3o4UrD53lo9CvFvVg"
            }, new System.Globalization.RegionInfo("nl")));
        }

        [Test]
        public async Task GetAlbumsFromArtistTest()
        {
            Assert.NotNull(await artistService.GetAlbumFromArtist("5cIc3SBFuBLVxJz58W2tU9"));
        }

        [Test]
        public async Task GetTopTracksTest()
        {
            Assert.NotNull(await artistService.GetTopTracks("5cIc3SBFuBLVxJz58W2tU9"));
        }

        [Test]
        public async Task GetRelatedArtistsTest()
        {
            Assert.NotNull(await artistService.GetRelatedArtists("5cIc3SBFuBLVxJz58W2tU9"));
        }


    }
}

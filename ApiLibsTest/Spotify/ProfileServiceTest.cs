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
    [Explicit]
    class ProfileServiceTest
    {
        private ProfileService profileService;
        private SpotifyService spotify;

        [OneTimeSetUp]
        public async Task SetUp()
        {
            Passwords passwords = await Passwords.ReadPasswords();
            spotify = new SpotifyService(passwords.SpotifyRefreshToken, passwords.SpotifyClientId, passwords.SpotifySecret);
            profileService = spotify.ProfileService;
        }

        [Test]
        public async Task GetMeTest()
        {
            Assert.NotNull(await profileService.GetMe());
        }

        [Test]
        [Ignore("Modifies State")]
        public async Task FollowArtistTest()
        {
            await profileService.Follow(UserType.Artist, "32WkQRZEVKSzVAAYqukAEA");
        }

        [Test]
        [Ignore("Modifies State")]
        public async Task UnfollowArtistTest()
        {
            await profileService.Unfollow(UserType.Artist, "32WkQRZEVKSzVAAYqukAEA");
        }

        [Test]
        [Ignore("Modifies State")]
        public async Task FollowUserTest()
        {
            await profileService.Follow(UserType.User, "ohwondermusic");
        }

        [Test]
        [Ignore("Modifies State")]
        public async Task UnfollowUserTest()
        {
            await profileService.Unfollow(UserType.User, "ohwondermusic");
        }

        [Test]
        [Ignore("Modifies State")]
        public async Task CheckIfFollowingTest()
        {
            await profileService.CheckIfFollowing(UserType.User, new List<string> { "ohwondermusic" });
        }

        [Test]
        public async Task GetFollowingArtistsTest()
        {
            Assert.NotNull(await profileService.GetFollowingArtists());
        }

        [Test]
        public async Task GetTopArtistsTest()
        {
            Assert.NotNull(await profileService.GetTopArtists());
        }

        //Tracks

        [Test]
        public async Task GetTopTracksTest()
        {
            Assert.NotNull(await profileService.GetTopTracks());
        }

        [Test]
        public async Task GetRecentlyPlayedTest()
        {
            Assert.NotNull(await profileService.GetRecentlyPlayed());
        }
    }
}

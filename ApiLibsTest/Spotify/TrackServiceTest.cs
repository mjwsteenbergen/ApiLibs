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
    class TrackServiceTest
    {
        private TrackService trackService;

        [OneTimeSetUp]
        public async Task SetUp()
        {
            Passwords passwords = await Passwords.ReadPasswords();
            SpotifyService spotify = new SpotifyService(passwords.SpotifyRefreshToken, passwords.SpotifyClientId, passwords.SpotifySecret);
            trackService = spotify.TrackService;
        }

        [Test]
        public async Task GetTrackTest()
        {
            Assert.AreEqual("Never Gonna Give You Up", (await trackService.GetTrack("4uLU6hMCjMI75M1A2tKUQC")).Name);
        }

        [Test]
        public async Task GetAudioAnalysisTest()
        {
            Assert.IsNotNull((await trackService.GetAudioAnalysis("4uLU6hMCjMI75M1A2tKUQC")).track);
        }

        [Test]
        public async Task GetAudioFeaturesTest()
        {
            Assert.IsNotNull((await trackService.GetAudioFeatures("4uLU6hMCjMI75M1A2tKUQC")).acousticness);
        }
    }
}

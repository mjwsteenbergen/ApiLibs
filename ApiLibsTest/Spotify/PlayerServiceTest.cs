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
    class PlayerServiceTest
    {
        private PlayerService playerService;
        private SpotifyService spotify;

        [OneTimeSetUp]
        public async Task SetUp()
        {
            Passwords passwords = await Passwords.ReadPasswords();
            spotify = new SpotifyService(passwords.SpotifyRefreshToken, passwords.SpotifyClientId, passwords.SpotifySecret);
            playerService = spotify.PlayerService;
        }

        [Test]
        public async Task GetDevicesTest()
        {
            Assert.IsNotNull(await playerService.GetDevices());
        }

        [Test]
        public async Task GetPlayerTest()
        {
            Assert.IsNotNull(await playerService.GetPlayer());
        }

        [Test]
        public async Task GetCurrentlyPlayingTest()
        {
            Assert.IsNotNull(await playerService.GetCurrentPlaying());
        }

        [Test]
        [Ignore("Modifies State")]
        public async Task TransferPlayback()
        {
            Device d = (await playerService.GetDevices()).devices[1];
            await playerService.TransferPlayback(d, false);
        }

        [Test]
        [Ignore("Modifies State")]
        public async Task PlayTest()
        {
            Track t = await spotify.TrackService.GetTrack("4uLU6hMCjMI75M1A2tKUQC");
            Device d = (await playerService.GetPlayer()).device;
            await playerService.Play(t, d);
        }

        [Test]
        [Ignore("Modifies State")]
        public async Task PlayAlbumTest()
        {
            Album album = await spotify.AlbumService.GetAlbum("30SqWqmSU9ww0Btb1j4rpU");
            await playerService.Play(album);
        }

        [Test]
        [Ignore("Modifies State")]
        public async Task PlayArtistTest()
        {
            await playerService.Play(new Artist { Id= "5Pwc4xIPtQLFEnJriah9YJ" });
        }

        [Test]
        [Ignore("Modifies State")]
        public async Task PlayPlaylistTest()
        {
            await playerService.Play(new Playlist { Id= "37i9dQZEVXcGwXcYmYDANi", Owner = new Owner {  Id= "onerepublicofficial" }  });
        }

        [Test]
        [Ignore("Modifies State")]
        public async Task PauseTest()
        {
            await playerService.Pause();
        }

        [Test]
        [Ignore("Modifies State")]
        public async Task NextTest()
        {
            await playerService.Next();
        }

        [Test]
        [Ignore("Modifies State")]
        public async Task NextWithIdTest()
        {
            var device = (await playerService.GetPlayer()).device;
            await playerService.Next(device.id);
        }

        [Test]
        [Ignore("Modifies State")]
        public async Task PreviousTest()
        {
            await playerService.Previous();
        }

        [Test]
        [Ignore("Modifies State")]
        public async Task PreviousWithIdTest()
        {
            var device = (await playerService.GetPlayer()).device;
            await playerService.Previous(device.id);
        }

        [Test]
        [Ignore("Modifies State")]
        public async Task SeekTest()
        {
            await playerService.Seek(0);
        }

        [Test]
        [Ignore("Modifies State")]
        public async Task SeekWithIdTest()
        {
            var device = (await playerService.GetPlayer()).device;
            await playerService.Seek(0, device.id);
        }

        [Test]
        [Ignore("Modifies State")]
        public async Task RepeatTest()
        {
            await playerService.Repeat(RepeatState.Off);
        }

        [Test]
        [Ignore("Modifies State")]
        public async Task RepeatAllTest()
        {
            await playerService.Repeat(RepeatState.Context);
            await playerService.Repeat(RepeatState.Track);
            await playerService.Repeat(RepeatState.Off);
        }

        [Test]
        [Ignore("Modifies State")]
        public async Task RepeatWithIdTest()
        {
            var device = (await playerService.GetPlayer()).device;
            await playerService.Repeat(RepeatState.Context, device.id);
        }

        [Ignore("Modifies State")]
        [Test]
        public async Task ShuffleTest()
        {
            await playerService.Shuffle(false);
        }

        [Ignore("Modifies State")]
        [Test]
        public async Task StartShuffleTest()
        {
            await playerService.Shuffle(true);
        }

        [Test]
        [Ignore("Modifies State")]
        public async Task StartShuffleWithIdTest()
        {
            var device = (await playerService.GetPlayer()).device;
            await playerService.Shuffle(true, device.id);
        }
    }
}

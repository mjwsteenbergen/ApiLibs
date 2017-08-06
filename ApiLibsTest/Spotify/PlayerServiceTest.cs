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
    class PlayerServiceTest
    {
        private PlayerService playerService;
        private SpotifyService spotify;

        [OneTimeSetUp]
        public void SetUp()
        {
            Passwords passwords = Passwords.ReadPasswords(Memory.ApplicationPath + "Laurentia" + Path.DirectorySeparatorChar);
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
        [Category("ModifyState")]
        public async Task TransferPlayback()
        {
            Device d = (await playerService.GetDevices()).devices[1];
            await playerService.TransferPlayback(d, false);
        }

        [Test]
        [Category("ModifyState")]
        public async Task PlayTest()
        {
            Track t = await spotify.TrackService.GetTrack("4uLU6hMCjMI75M1A2tKUQC");
            Device d = (await playerService.GetPlayer()).device;
            await playerService.Play(t, d);
        }

        [Test]
        [Category("ModifyState")]
        public async Task PlayAlbumTest()
        {
            Album album = await spotify.AlbumService.GetAlbum("30SqWqmSU9ww0Btb1j4rpU");
            await playerService.Play(album);
        }

        [Test]
        [Category("ModifyState")]
        public async Task PlayArtistTest()
        {
            await playerService.Play(new Artist { id= "5Pwc4xIPtQLFEnJriah9YJ" });
        }

        [Test]
        [Category("ModifyState")]
        public async Task PlayPlaylistTest()
        {
            await playerService.Play(new Playlist { id= "37i9dQZEVXcGwXcYmYDANi", owner = new Owner {  id= "onerepublicofficial" }  });
        }

        [Test]
        [Category("ModifyState")]
        public async Task PauseTest()
        {
            await playerService.Pause();
        }

        [Test]
        [Category("ModifyState")]
        public async Task NextTest()
        {
            await playerService.Next();
        }

        [Test]
        [Category("ModifyState")]
        public async Task NextWithIdTest()
        {
            var device = (await playerService.GetPlayer()).device;
            await playerService.Next(device.id);
        }

        [Test]
        [Category("ModifyState")]
        public async Task PreviousTest()
        {
            await playerService.Previous();
        }

        [Test]
        [Category("ModifyState")]
        public async Task PreviousWithIdTest()
        {
            var device = (await playerService.GetPlayer()).device;
            await playerService.Previous(device.id);
        }

        [Test]
        [Category("ModifyState")]
        public async Task SeekTest()
        {
            await playerService.Seek(0);
        }

        [Test]
        [Category("ModifyState")]
        public async Task SeekWithIdTest()
        {
            var device = (await playerService.GetPlayer()).device;
            await playerService.Seek(0, device.id);
        }

        [Test]
        [Category("ModifyState")]
        public async Task RepeatTest()
        {
            await playerService.Repeat(RepeatState.Off);
        }

        [Test]
        [Category("ModifyState")]
        public async Task RepeatAllTest()
        {
            await playerService.Repeat(RepeatState.Context);
            await playerService.Repeat(RepeatState.Track);
            await playerService.Repeat(RepeatState.Off);
        }

        [Test][Category("ModifyState")]
        public async Task RepeatWithIdTest()
        {
            var device = (await playerService.GetPlayer()).device;
            await playerService.Repeat(RepeatState.Context, device.id);
        }

        [Category("ModifyState")]
        [Test]
        public async Task ShuffleTest()
        {
            await playerService.Shuffle(false);
        }

        [Category("ModifyState")]
        [Test]
        public async Task StartShuffleTest()
        {
            await playerService.Shuffle(true);
        }

        [Test]
        [Category("ModifyState")]
        public async Task StartShuffleWithIdTest()
        {
            var device = (await playerService.GetPlayer()).device;
            await playerService.Shuffle(true, device.id);
        }
    }
}

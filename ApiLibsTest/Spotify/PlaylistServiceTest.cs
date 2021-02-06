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
    class PlaylistServiceTest
    {
        private PlaylistService playlistService;
        private SpotifyService spotify;

        [OneTimeSetUp]
        public async Task SetUp()
        {
            Passwords passwords = await Passwords.ReadPasswords();
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

        [Test]
        public async Task CheckAddMoreThan100()
        {
            var res = await spotify.LibraryService.GetMySavedTracks(0, 50);
            var res2 = await spotify.LibraryService.GetMySavedTracks(50, 50);
            var res3 = await spotify.LibraryService.GetMySavedTracks(100, 50);

            var playlist = await playlistService.CreatePlaylist("newnottakenname", "longplaylist");
            await playlistService.AddTracks(res.Items.Select(i => i.Track).Concat(res2.Items.Select(i => i.Track)).Concat(res3.Items.Select(i => i.Track)), playlist);

            await playlistService.DeletePlaylist(playlist);
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiLibs.General;
using ApiLibs.Spotify;
using NUnit.Framework;
using RestSharp.Authenticators.OAuth;

namespace ApiLibsTest.Spotify
{
    [Explicit]
    class SpotifyTest
    {
        private SpotifyService spotify;

        [OneTimeSetUp]
        public async Task SetUp()
        {
            Passwords passwords = await Passwords.ReadPasswords();
            spotify = new SpotifyService(passwords.SpotifyRefreshToken, passwords.SpotifyClientId, passwords.SpotifySecret);
        }

        #region auth 

        [Test]
        [Ignore("Modifies State")]
        public async Task Login()
        {
            Passwords passwords = await Passwords.ReadPasswords();
            spotify.Connect(new StupidOAuth(), passwords.SpotifyClientId, "https://www.nntn.nl/", new List<Scope>()
            {
                Scope.UserLibraryModify,Scope.PlaylistModifyPrivate,Scope.UserFollowRead,Scope.UserReadPrivate,Scope.UserTopRead
            });
        }

        [Test]
        [Ignore("Modifies State")]
        public async Task GetKey()
        {
            spotify = new SpotifyService();
            Passwords passwords = await Passwords.ReadPasswords();
            string token =
                "";
            var s = await spotify.ConvertToToken(token, "https://www.nntn.nl/", passwords.SpotifyClientId,
                passwords.SpotifySecret);
        }


        #endregion
        #region Search

        [Test]
        public async Task SearchTrackTest()
        {
            var result = await spotify.Search("Never gonna give you up", SpotifyService.SearchType.Track);
            Assert.IsNotEmpty(result.tracks.Items);
        }

        [Test]
        public async Task SearchArtistTest()
        {
            var result = await spotify.Search("Rick Astley", SpotifyService.SearchType.Artist);
            Assert.IsNotEmpty(result.artists.Items);
        }

        [Test]
        public async Task SearchAlbumTest()
        {
            var result = await spotify.Search("Whenever you need somebody", SpotifyService.SearchType.Album);
            Assert.IsNotEmpty(result.albums.Items);
        }

        [Test]
        public async Task SearchPlayListTest()
        {
            var result = await spotify.Search("Rick Astley: Top Tracks", SpotifyService.SearchType.Playlist);
            Assert.IsNotEmpty(result.playlists.Items);
        }
    #endregion
    }
}

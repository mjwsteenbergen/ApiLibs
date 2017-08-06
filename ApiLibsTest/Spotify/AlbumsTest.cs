using System.IO;
using System.Threading.Tasks;
using ApiLibs.General;
using ApiLibs.Spotify;
using NUnit.Framework;

namespace ApiLibsTest.Spotify
{
    class AlbumsTest
    {
        private AlbumService albumService;
        private SpotifyService spotify;

        [OneTimeSetUp]
        public void SetUp()
        {
            Passwords passwords = Passwords.ReadPasswords(Memory.ApplicationPath + "Laurentia" + Path.DirectorySeparatorChar);
            spotify = new SpotifyService(passwords.SpotifyRefreshToken, passwords.SpotifyClientId, passwords.SpotifySecret);
            albumService = spotify.AlbumService;
        }

        [Test]
        public async Task GetAlbumTest()
        {
            Assert.NotNull(await albumService.GetAlbum("1qnHtqA3ZLLxFnUid3VseY"));
        }

        [Test]
        public async Task GetNewReleasesTest()
        {
            Assert.NotNull(await albumService.GetNewReleases());
        }

        [Test]
        public async Task GetTracksTest()
        {
            Album alb = await albumService.GetAlbum("1qnHtqA3ZLLxFnUid3VseY");
            Assert.NotNull(await albumService.GetTracks(alb));
        }
    }
}

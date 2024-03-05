using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiLibs.General;

namespace ApiLibs.Spotify
{
    public class ArtistService : SubService<SpotifyService>
    {
        public ArtistService(SpotifyService service) : base(service)
        {
        }

        public async Task<Artist> GetArtist(string id)
        {
            return await MakeRequest<Artist>("artists/" + id);
        }

        public async Task<List<Artist>> GetArtists(List<string> ids, RegionInfo info, List<AlbumType> types = null, int? limit = null, int? offset = null)
        {
            return (await MakeRequest<RelatedArtistResult>("artists?ids=" + ids.Aggregate((i,j) => i + "," + j), parameters: new List<Param>
            {
                new Param("country", info.TwoLetterISORegionName),
                new OParam("types", types?.ConvertAll<string>(i => i.ToString().ToLower()).Aggregate((i, j) => i + "," + j)),
                new OParam("limit", limit),
                new OParam("offset", offset)
            })).artists;
        }

        public Task<List<Track>> GetTopTracks(Artist artist, RegionInfo info = null) => GetTopTracks(artist.Id, info);

        public async Task<List<Album>> GetAlbumFromArtist(Artist artist, string includeGroups = null, int? limit = null, int? offset = null)
        {
            return await GetAlbumFromArtist(artist.Id, includeGroups, limit, offset);
        }

        public async Task<List<Album>> GetAlbumFromArtist(string artistId, string includeGroups = null, int? limit = null, int? offset = null)
        {
            return (await MakeRequest<AlbumResultsResponse>("artists/" + artistId + "/albums", parameters:
            new List<Param> {
                new OParam("include_groups", includeGroups),
                new OParam("limit", limit),
                new OParam("offset", offset)
            })).Items;
        }

        public async Task<List<Track>> GetTopTracks(string artistId, RegionInfo info = null)
        {
            return (await MakeRequest<TrackResponse>("artists/" + artistId + "/top-tracks", parameters: new List<Param> {
                new Param("country", info?.TwoLetterISORegionName ?? "from_token"),
            })).Tracks;
        }

        public async Task<List<Artist>> GetRelatedArtists(string id)
        {
            return (await MakeRequest<RelatedArtistResult>("artists/" + id + "/related-artists")).artists;
        }

        private class RelatedArtistResult
        {
            public List<Artist> artists { get; set; }
        }
    }

    public enum UserType
    {
        User, Artist
    }

    public enum AlbumType
    {
        Album, Single, Appears_On, Compilation
    }
}

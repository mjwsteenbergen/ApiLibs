using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiLibs.General;
using Martijn.Extensions.AsyncLinq;
using Martijn.Extensions.Linq;
using RestSharp;

namespace ApiLibs.Spotify
{
    public class TrackService : SubService<SpotifyService>
    {
        public TrackService(SpotifyService service) : base(service) { }

        public async Task<Track> GetTrack(string id)
        {
            return await MakeRequest<Track>("tracks/" + id);
        }

        public async Task<List<Track>> GetTracks(List<string> ids)
        {
            return await MakeRequest<List<Track>>("tracks", parameters: new List<Param>
            {
                new Param("ids", ids.Aggregate((i,j) => i + "," + j))
            });
        }

        public async Task<AudioAnalysis> GetAudioAnalysis(string id)
        {
            return await MakeRequest<AudioAnalysis>("audio-analysis/" + id);
        }

        public async Task<AudioFeatures> GetAudioFeatures(string id)
        {
            return await MakeRequest<AudioFeatures>("audio-features/" + id);
        }


        public async Task<AudioFeatureList> GetAudioFeatures(IEnumerable<Track> tracks)
        {
            return new AudioFeatureList {
                AudioFeatures = await GetAudioFeaturesAsync(tracks).ToList()
            };
        }

        public IAsyncEnumerable<AudioFeatures> GetAudioFeaturesAsync(IEnumerable<Track> tracks)
        {
            return tracks
            .Split(20)
            .Select(i => MakeRequest<AudioFeatureList>("audio-features/?ids=" + i.Select(j => j.Id).Combine(","))).ToIAsyncEnumberable()
            .SelectMany(i => i.AudioFeatures);
        }
    }
}

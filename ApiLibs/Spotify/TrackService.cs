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

    public static class LinqHelpers
    {
        public static async Task<List<T>> ToList<T>(this IAsyncEnumerable<T> enumerable)
        {
            List<T> res = new List<T>();
            await foreach (var item in enumerable)
            {
                res.Add(item);
            }
            return res;
        }

        public static async Task<T> First<T>(this IAsyncEnumerable<T> enumerable, Func<T, bool> func)
        {
            await foreach (var item in enumerable)
            {
                if(func(item))
                {
                    return item;
                }
            }

            throw new InvalidOperationException("Sequence contains no matching element");
        }

        public static Task<T> First<T>(this IAsyncEnumerable<T> enumerable) => enumerable.First(i => true);

        public static async IAsyncEnumerable<Y> SelectMany<T,Y>(this IAsyncEnumerable<T> enumerable, Func<T, IEnumerable<Y>> func)
        {
            await foreach (var item in enumerable)
            {
                foreach(var actualItem in func(item))
                {
                    yield return actualItem;
                }
            }
        }

        public static async IAsyncEnumerable<T> TakeWhile<T>(this IAsyncEnumerable<T> enumerable, Func<T, bool> func)
        {
            await foreach (var item in enumerable)
            {
                if(!func(item))
                {
                    break;
                }
                yield return item;
            }
        }

        public static IEnumerable<T> NotNull<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.Where(i => i != null);
        }

        public static IEnumerable<List<T>> Subset<T>(this IEnumerable<T> list, int amountOfItems)
        {
            List<List<T>> cache = new List<List<T>>();

            foreach (var item in list)
            {
                cache = cache.Concat(cache.Select(i => i.Concat(new List<T> { item }).ToList())).Append(new List<T> { item }).ToList();
                foreach (var res in cache.Where(i => i.Count == amountOfItems))
                {
                    yield return res;
                }
                cache = cache.Where(i => i.Count < amountOfItems).ToList();
            }
        }


        public static IEnumerable<List<T>> Split<T>(this IEnumerable<T> list, int size)
        {
            if (size < 0) throw new ArgumentException(nameof(size) + " should be bigger than zero");
            List<T> subset = new List<T>();
            foreach (var item in list)
            {
                subset.Add(item);
                if (subset.Count >= size)
                {
                    yield return subset;
                    subset = new List<T>();
                }
            }

            if(subset.Count > 0)
            {
                yield return subset;
            }
        }
    }
}

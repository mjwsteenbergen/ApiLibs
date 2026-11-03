using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using ParityTesting;

namespace ApiLibs.JSONTest
{
    /// <summary>
    /// JSON written by hand for shapes the generator cannot produce: values the APIs send that
    /// do not round-trip through the C# classes (Reddit's "edited" is false or a timestamp),
    /// and the input of custom converters. Each blob is also mutated like the generated ones.
    /// </summary>
    [Category("NoInternet")]
    public class HandWrittenParityTests
    {
        public static readonly (Type type, string name, string json)[] Fixtures =
        {
            (typeof(ApiLibs.Reddit.SubredditResult), "edited false and timestamp", """
                {"kind":"Listing","data":{"after":"t3_x","dist":2,"modhash":"","children":[
                  {"kind":"t3","data":{"id":"a","title":"Post","score":12,"edited":false,"created_utc":1700000000.0,"approved_by":null,"author_flair_richtext":[],"thumbnail":"self"}},
                  {"kind":"t3","data":{"id":"b","title":"Edited","score":-1,"edited":1700000123.0,"created_utc":1700000001,"author_flair_richtext":[{"e":"text","t":"flair"}]}}]}}
                """),
            (typeof(ApiLibs.Reddit.SubredditResult), "edited as an integer", """
                {"kind":"Listing","data":{"children":[{"kind":"t3","data":{"id":"c","edited":1700000123}}]}}
                """),
            (typeof(ApiLibs.Reddit.CommentsResult), "comment edited true", """
                {"kind":"Listing","data":{"children":[{"kind":"t1","data":{"id":"c1","body":"text","edited":true,"replies":""}}]}}
                """),
            (typeof(ApiLibs.Pocket.ReadingList), "get.php after PocketService's regex rewrite", """
                {"status":1,"complete":1,"list":[{"item_id":"229279689","resolved_id":"229279689","given_url":"http://www.example.com/article","given_title":"The Title","favorite":"0","status":"0","time_added":"1700000000","time_updated":"1700000100","time_read":"0","time_favorited":"0","sort_id":0,"resolved_title":"The Resolved Title","resolved_url":"http://www.example.com/article","excerpt":"Text","is_article":"1","is_index":"0","has_video":"1","has_image":"1","word_count":"3197","tags":{"read-later":{"item_id":"229279689","tag":"read-later"}},"authors":[{"item_id":"229279689","author_id":"1","name":"Author","url":"http://www.example.com/a"}],"image":{"item_id":"229279689","src":"http://www.example.com/i.jpg","width":"0","height":"0"},"images":[{"item_id":"229279689","image_id":"1","src":"http://www.example.com/i.jpg","width":"0","height":"0","credit":"","caption":""}],"videos":[{"item_id":"229279689","video_id":"1","src":"http://www.youtube.com/v/x","width":"0","height":"0","type":"1","vid":"x"}]}],"error":null,"search_meta":{"search_type":"normal"},"since":1700000200}
                """),
            (typeof(ApiLibs.Todoist.SyncResult), "sync_status with ok and an error", """
                {"sync_status":{"2c9a5ea2-4c4e-4d2a-9a8a-3d4c2b1a0f11":"ok","7f1e2d3c-1b2a-4c5d-8e9f-0a1b2c3d4e5f":{"error_code":15,"error":"Invalid temporary id"}},"temp_id_mapping":{"a":1}}
                """),
        };

        public static IEnumerable<TestCaseData> Cases() => Fixtures
            .Where(f => Apis.IsPorted(f.type))
            .Select(f => new TestCaseData(f.type, f.json).SetName($"{TypeNames.Of(f.type).Replace("ApiLibs.", "")} ({f.name})"));

        [TestCaseSource(nameof(Cases))]
        public void Read(Type oldType, string json)
        {
            var failures = new List<string>();
            foreach (var (name, variant) in Mutations.Of(json))
            {
                var difference = Parsing.Compare(variant, oldType);
                if (difference != null)
                {
                    failures.Add($"[{name}] {difference}");
                }
            }
            Assert.That(failures, Is.Empty, string.Join("\n", failures.Take(15)));
        }
    }
}

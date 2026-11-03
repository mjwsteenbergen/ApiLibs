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

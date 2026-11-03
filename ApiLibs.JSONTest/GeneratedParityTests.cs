using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using ParityTesting;

namespace ApiLibs.JSONTest
{
    /// <summary>
    /// For every return class of every ported service: AutoFixture fills an instance of the old
    /// class with a fixed seed, Newtonsoft serializes it to a blob, the blob is mutated (null,
    /// removed, emptied, unknown fields), and each variant must parse to the same object in
    /// ApiLibs and ApiLibs.JSON. The parsed objects must also serialize to the same JSON when
    /// sent as a request body or parameter.
    /// </summary>
    [Category("NoInternet")]
    public class GeneratedParityTests
    {
        private const int Seeds = 3;

        public static IEnumerable<TestCaseData> DataTypes() =>
            Apis.DataTypes().Select(t => new TestCaseData(t).SetName($"{TypeNames.Of(t).Replace("ApiLibs.", "")}"));

        public static Fixture CreateFixture(int seed)
        {
            var fixture = SeededFixture.Create(seed);
            fixture.Register<JToken>(() => new JValue("token"));
            fixture.Register<JObject>(() => new JObject { ["key"] = "value" });
            return fixture;
        }

        /// <summary>Base blobs for a type, one per seed. Types AutoFixture cannot build get none.</summary>
        public static List<string> BaseBlobs(Type oldType, out string? problem)
        {
            problem = null;
            var blobs = new List<string>();
            for (int seed = 0; seed < Seeds; seed++)
            {
                try
                {
                    var instance = SeededFixture.CreateInstance(CreateFixture(SeededFixture.Seed + seed), oldType);
                    blobs.Add(Newtonsoft.Json.JsonConvert.SerializeObject(instance));
                }
                catch (Exception e)
                {
                    problem = e.GetType().Name + ": " + e.Message.Split('\n')[0];
                }
            }
            return blobs;
        }

        [TestCaseSource(nameof(DataTypes))]
        public void Read(Type oldType)
        {
            var blobs = BaseBlobs(oldType, out var problem);
            if (blobs.Count == 0)
            {
                Assert.Fail($"No generated fixture: {problem}. Add a hand-written fixture instead.");
            }

            var failures = new List<string>();
            foreach (var blob in blobs)
            {
                foreach (var (name, json) in Mutations.Of(blob))
                {
                    var difference = Parsing.Compare(json, oldType);
                    if (difference != null)
                    {
                        failures.Add($"[{name}] {difference}\n      {Truncate(json)}");
                    }
                }
            }

            Assert.That(failures, Is.Empty, $"{failures.Count} variants differ:\n" + string.Join("\n", failures.Take(15)));
        }

        [TestCaseSource(nameof(DataTypes))]
        public void Write(Type oldType)
        {
            var newType = Apis.NewTypeFor(oldType)!;
            var failures = new List<string>();
            foreach (var blob in BaseBlobs(oldType, out _))
            {
                var (oldValue, oldError) = Parsing.Try(() => Parsing.Old(blob, oldType));
                var (newValue, newError) = Parsing.Try(() => Parsing.New(blob, newType));
                if (oldError != null || newError != null)
                {
                    continue;
                }

                // Param(name, object) is the serialization used for request bodies and parameters.
                var (oldText, oldWriteError) = Parsing.Try(() => new ApiLibs.Param("p", oldValue).Value);
                var (newText, newWriteError) = Parsing.Try(() => new ApiLibs.JSON.Param("p", newValue).Value);
                if (oldWriteError != null || newWriteError != null)
                {
                    if (oldWriteError == null || newWriteError == null)
                    {
                        failures.Add($"write: {Parsing.Outcome(oldText, oldWriteError, newText, newWriteError)}");
                    }
                    continue;
                }

                var difference = JsonTrees.Difference(JsonTrees.Parse((string)oldText!), JsonTrees.Parse((string)newText!), "$");
                if (difference != null)
                {
                    failures.Add($"{difference}\n      old: {Truncate((string)oldText!)}\n      new: {Truncate((string)newText!)}");
                }
            }

            Assert.That(failures, Is.Empty, string.Join("\n", failures.Take(10)));
        }

        private static string Truncate(string s) => s.Length > 400 ? s.Substring(0, 400) + "…" : s;
    }
}

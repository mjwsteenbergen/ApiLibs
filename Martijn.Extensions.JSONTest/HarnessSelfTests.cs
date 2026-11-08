using System;
using System.Collections.Generic;
using NUnit.Framework;
using ParityTesting;

namespace Martijn.Extensions.JSONTest
{
    /// <summary>The comparers must report differences, or every parity test passes trivially.</summary>
    [Category("NoInternet")]
    public class HarnessSelfTests
    {
        [Test]
        public void DeepComparerFindsDifferences()
        {
            Assert.That(DeepComparer.Compare(new Nested { Id = 1 }, new Nested { Id = 1 }), Is.Null);
            Assert.That(DeepComparer.Compare(new Nested { Id = 1 }, new Nested { Id = 2 }), Is.Not.Null);
            Assert.That(DeepComparer.Compare(new Nested { Name = "a" }, new Nested { Name = null }), Is.Not.Null);
            Assert.That(DeepComparer.Compare(new List<int> { 1 }, new List<int> { 1, 2 }), Is.Not.Null);
            Assert.That(DeepComparer.Compare(new List<Nested> { new() { Id = 1 } }, new List<Nested> { new() { Id = 3 } }), Is.Not.Null);
            Assert.That(DeepComparer.Compare(new Dictionary<string, int> { ["a"] = 1 }, new Dictionary<string, int> { ["a"] = 2 }), Is.Not.Null);
            Assert.That(DeepComparer.Compare(new DateTime(1, DateTimeKind.Utc), new DateTime(1, DateTimeKind.Local)), Is.Not.Null);
            Assert.That(DeepComparer.Compare(1L, 1), Is.Not.Null);
            Assert.That(DeepComparer.Compare(Colour.Red, Colour.Blue), Is.Not.Null);
            Assert.That(DeepComparer.Compare((1, "a"), (1, "b")), Is.Not.Null);
            Assert.That(DeepComparer.Compare(new Holder<object> { V = new Nested() }, new Holder<object> { V = new Dog() }), Is.Not.Null);

            var element = System.Text.Json.JsonDocument.Parse("{\"a\":1}").RootElement;
            Assert.That(DeepComparer.Compare(Newtonsoft.Json.Linq.JToken.Parse("{\"a\":1.0}"), element), Is.Null);
            Assert.That(DeepComparer.Compare(Newtonsoft.Json.Linq.JToken.Parse("{\"a\":2}"), element), Is.Not.Null);
        }

        [Test]
        public void JsonTreesFindDifferences()
        {
            Assert.That(JsonTrees.Difference(JsonTrees.Parse("{\"a\":1,\"b\":[1]}"), JsonTrees.Parse("{\"b\":[1.0],\"a\":1}"), "$"), Is.Null);
            Assert.That(JsonTrees.Difference(JsonTrees.Parse("{\"a\":1}"), JsonTrees.Parse("{\"a\":1,\"b\":null}"), "$"), Is.Not.Null);
            Assert.That(JsonTrees.Difference(JsonTrees.Parse("{\"a\":\"2020-01-01T00:00:00Z\"}"), JsonTrees.Parse("{\"a\":\"2020-01-01T00:00:00.0Z\"}"), "$"), Is.Not.Null);
            Assert.That(JsonTrees.Difference(JsonTrees.Parse("[1,2]"), JsonTrees.Parse("[2,1]"), "$"), Is.Not.Null);
        }
    }
}

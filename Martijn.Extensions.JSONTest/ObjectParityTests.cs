using System;
using System.Collections.Generic;
using System.Linq;
using Martijn.Extensions.JSON.Compat;
using NUnit.Framework;
using ParityTesting;
using N = Newtonsoft.Json;
using S = System.Text.Json.Serialization;

namespace Martijn.Extensions.JSONTest
{
    // Test classes carry the Newtonsoft attribute and its System.Text.Json replacement, the way
    // the converted ApiLibs classes do, so one class can go through both serializers.

    public class Attributed
    {
        [N.JsonProperty("the_name")]
        [S.JsonPropertyName("the_name")]
        public string? Name { get; private set; }

        [N.JsonProperty("count", NullValueHandling = N.NullValueHandling.Ignore)]
        [S.JsonPropertyName("count")]
        [NullValueHandling(NullHandling.Ignore)]
        public int? Count { get; set; } = 5;

        [N.JsonProperty("label", NullValueHandling = N.NullValueHandling.Ignore)]
        [S.JsonPropertyName("label")]
        [NullValueHandling(NullHandling.Ignore)]
        public string? Label { get; set; } = "default";

        [N.JsonProperty("always", NullValueHandling = N.NullValueHandling.Include)]
        [S.JsonPropertyName("always")]
        [NullValueHandling(NullHandling.Include)]
        public string? Always { get; set; }

        [N.JsonProperty("level", NullValueHandling = N.NullValueHandling.Ignore)]
        [S.JsonPropertyName("level")]
        [NullValueHandling(NullHandling.Ignore)]
        public int Level { get; set; } = 3;

        [N.JsonIgnore]
        [S.JsonIgnore]
        public string? Hidden { get; set; } = "hidden";

        public string? NoAttribute { get; set; }

        public int PrivateSetWithoutAttribute { get; private set; } = 7;

        public string Computed => "computed-" + Name;

        public List<string> Tags { get; set; } = new();

        public int Field;

        public readonly int ReadonlyField = 9;
    }

    public class Animal
    {
        public string? Name { get; set; }
    }

    public class Dog : Animal
    {
        public bool Barks { get; set; }
    }

    public interface IShape
    {
        int Sides { get; }
    }

    public class Square : IShape
    {
        public int Sides { get; set; } = 4;
        public double Size { get; set; }
    }

    public class Zoo
    {
        public Animal? Star { get; set; }
        public List<Animal>? Animals { get; set; }
        public Animal[]? AnimalArray { get; set; }
        public Dictionary<string, Animal>? ByName { get; set; }
        public IShape? Shape { get; set; }
        public object? Anything { get; set; }
        public List<object>? Things { get; set; }
    }

    public class WithConstructor
    {
        public WithConstructor(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; }
        public string Name { get; }
        public string? Extra { get; set; }
    }

    public class SyncState
    {
        public List<(Attributed? first, Animal? second, DateTime when)> Pairs { get; set; } = new();
        public Dictionary<string, object> Bag { get; set; } = new();
        public Dictionary<string, List<int>> Lists { get; set; } = new();
        public (int, string) Tuple { get; set; }
    }

    public class PrivateMembers
    {
        [N.JsonIgnore]
        [S.JsonIgnore]
        public List<string>? Tags { get; set; }

        [N.JsonProperty("tags")]
        [S.JsonPropertyName("tags")]
        private List<string>? TagReferences { get => Tags; set => Tags = value; }

        [N.JsonProperty("computed")]
        [S.JsonPropertyName("computed")]
        private int Computed => Tags?.Count ?? -1;

        [N.JsonProperty("secret")]
        [S.JsonPropertyName("secret")]
        private string? secret;

        public string? GetSecret() => secret;

        public void SetSecret(string value) => secret = value;
    }

    public struct BoolOrNumber
    {
        public bool? Bool;
        public double? Double;

        public static implicit operator BoolOrNumber(bool value) => new BoolOrNumber { Bool = value };
        public static implicit operator BoolOrNumber(double value) => new BoolOrNumber { Double = value };
    }

    public class Wrapped
    {
        public string? Text { get; set; }

        public static explicit operator Wrapped(string value) => new Wrapped { Text = "cast:" + value };
    }

    public class WithCasts
    {
        public BoolOrNumber Edited { get; set; }
        public BoolOrNumber? MaybeEdited { get; set; }
        public Wrapped? Wrapped { get; set; }
        public List<BoolOrNumber>? Many { get; set; }
    }

    public class Style
    {
        public string? Font { get; set; } = "Arial";
        public int Size { get; set; } = 11;
    }

    public class Prefilled
    {
        public Prefilled()
        {
            Main = new Style();
            Fixed.Add("from constructor");
        }

        public Style Main { get; private set; }
        public Style? Missing { get; private set; }
        public List<string> Fixed { get; } = new();
        public List<string> Settable { get; set; } = new() { "initial" };
        public Style Computed => new Style();
    }

    [Category("NoInternet")]
    public class ObjectParityTests
    {
        public static readonly (Type type, string json)[] ReadCases =
        {
            (typeof(Attributed), "{}"),
            (typeof(Attributed), "{\"the_name\":\"n\",\"count\":1,\"label\":\"l\",\"always\":\"a\",\"level\":2}"),
            (typeof(Attributed), "{\"THE_NAME\":\"upper\",\"Count\":\"12\",\"LABEL\":null,\"level\":null}"),
            (typeof(Attributed), "{\"the_name\":null,\"count\":null,\"label\":null,\"always\":null}"),
            (typeof(Attributed), "{\"Hidden\":\"x\",\"NoAttribute\":\"y\",\"noattribute\":\"z\",\"PrivateSetWithoutAttribute\":1,\"Computed\":\"c\"}"),
            (typeof(Attributed), "{\"Tags\":[\"a\",\"b\"],\"Field\":4,\"ReadonlyField\":5}"),
            (typeof(Attributed), "{\"Tags\":null}"),
            (typeof(Attributed), "{\"Tags\":\"\"}"),
            (typeof(Attributed), "{\"the_name\":\"a\",\"the_name\":\"b\"}"),
            (typeof(Attributed), "{\"unknown\":{\"deep\":[1,2,{\"x\":null}]},\"the_name\":\"after\"}"),
            (typeof(Attributed), "{/* comment */\"the_name\":\"c\",}"),
            (typeof(Attributed), "{\"count\":1.5}"),
            (typeof(Attributed), "[]"),
            (typeof(Attributed), "\"\""),
            (typeof(Attributed), "\"text\""),
            (typeof(Attributed), "null"),
            (typeof(Zoo), "{\"Star\":{\"Name\":\"rex\",\"Barks\":true},\"Animals\":[{\"Name\":\"a\"},null,\"\"],\"ByName\":{\"x\":{\"Name\":\"x\"},\"y\":\"\"}}"),
            (typeof(Zoo), "{\"Star\":\"\",\"Animals\":\"\",\"ByName\":\"\",\"AnimalArray\":\"\"}"),
            (typeof(Zoo), "{\"Anything\":{\"a\":[1,\"2020-01-01T00:00:00Z\",true]},\"Things\":[1,1.5,\"s\",\"2020-01-01T10:00:00+02:00\",null,true,[],{}]}"),
            (typeof(Zoo), "{\"Anything\":12345678901234567890123}"),
            (typeof(Zoo), "{\"Shape\":{\"Sides\":3}}"),
            (typeof(Zoo), "{\"Shape\":null}"),
            (typeof(PrivateMembers), "{\"tags\":[\"a\"],\"computed\":5,\"secret\":\"s\"}"),
            (typeof(PrivateMembers), "{\"TAGS\":null,\"Secret\":\"public name\"}"),
            (typeof(WithCasts), "{\"Edited\":false,\"MaybeEdited\":1700000000.5,\"Wrapped\":\"w\",\"Many\":[true,2.5,{\"Bool\":true}]}"),
            (typeof(WithCasts), "{\"Edited\":1700000000,\"MaybeEdited\":null}"),
            (typeof(WithCasts), "{\"Edited\":\"text\"}"),
            (typeof(WithCasts), "{\"Edited\":{\"Bool\":true,\"Double\":null},\"MaybeEdited\":\"\",\"Wrapped\":{\"Text\":\"t\"}}"),
            (typeof(WithCasts), "{\"Edited\":null}"),
            (typeof(BoolOrNumber), "true"),
            (typeof(BoolOrNumber?), "\"\""),
            (typeof(Wrapped), "\"root\""),
            (typeof(Prefilled), "{\"Main\":{\"Size\":14},\"Missing\":{\"Font\":\"x\"},\"Fixed\":[\"json\"],\"Settable\":[\"json\"],\"Computed\":{\"Size\":1}}"),
            (typeof(Prefilled), "{\"Main\":null,\"Missing\":null,\"Fixed\":null,\"Settable\":null}"),
            (typeof(Prefilled), "{\"Fixed\":\"\",\"Settable\":\"\"}"),
            (typeof(WithConstructor), "{\"id\":1,\"name\":\"n\",\"extra\":\"e\"}"),
            (typeof(WithConstructor), "{\"Id\":\"2\",\"Extra\":\"e\"}"),
            (typeof(WithConstructor), "{}"),
            (typeof(SyncState), "{\"Pairs\":[{\"Item1\":{\"the_name\":\"a\"},\"Item2\":{\"Name\":\"b\"},\"Item3\":\"2021-05-06T07:08:09\"}],\"Bag\":{\"n\":1,\"s\":\"x\",\"o\":{\"k\":1},\"d\":\"2021-05-06T07:08:09Z\"},\"Lists\":{\"a\":[1,2],\"b\":\"\"},\"Tuple\":{\"Item1\":1,\"Item2\":\"x\"}}"),
            (typeof(SyncState), "{\"Pairs\":[{\"Item1\":null,\"Item2\":null}],\"Tuple\":null}"),
        };

        public static IEnumerable<TestCaseData> Reads() => ReadCases.Select((c, i) => new TestCaseData(c.type, c.json).SetName($"Read {c.type.Name} #{i}"));

        [TestCaseSource(nameof(Reads))]
        public void ReadsTheSame(Type type, string json)
        {
            Assert.That(Parity.CompareRead(json, type), Is.Null, json);
        }

        public static IEnumerable<TestCaseData> Writes()
        {
            var attributed = (Attributed)N.JsonConvert.DeserializeObject("{\"the_name\":\"n\",\"Tags\":[\"t\"]}", typeof(Attributed))!;
            attributed.Count = null;
            attributed.Label = null;
            attributed.Field = 3;

            yield return new TestCaseData(attributed).SetName("Write Attributed");
            yield return new TestCaseData(new Attributed()).SetName("Write Attributed defaults");
            yield return new TestCaseData(new Zoo
            {
                Star = new Dog { Name = "rex", Barks = true },
                Animals = new List<Animal> { new Dog { Name = "a" }, new Animal { Name = "b" }, null! },
                AnimalArray = new Animal[] { new Dog() },
                ByName = new Dictionary<string, Animal> { ["d"] = new Dog { Barks = true } },
                Shape = new Square { Size = 2.5 },
                Anything = new Dog { Name = "boxed" },
                Things = new List<object> { 1, 1.0, 1.5m, "s", DateTime.MinValue, new Dog(), new object(), null! },
            }).SetName("Write Zoo");
            yield return new TestCaseData(new SyncState
            {
                Pairs = { (new Attributed(), new Dog(), new DateTime(2020, 1, 2, 3, 4, 5, DateTimeKind.Local)), (null, null, default) },
                Bag = { ["a"] = 1L, ["b"] = new List<int> { 1 }, ["c"] = new Dictionary<string, object> { ["x"] = 2.0 } },
                Lists = { ["l"] = new List<int> { 1, 2 } },
                Tuple = (1, "one"),
            }).SetName("Write SyncState");
            yield return new TestCaseData(new WithConstructor(1, "c") { Extra = null }).SetName("Write WithConstructor");
            var privateMembers = new PrivateMembers { Tags = new List<string> { "x" } };
            privateMembers.SetSecret("s");
            yield return new TestCaseData(privateMembers).SetName("Write PrivateMembers");
            yield return new TestCaseData(new PrivateMembers()).SetName("Write PrivateMembers empty");
        }

        [TestCaseSource(nameof(Writes))]
        public void WritesTheSame(object value)
        {
            AssertSameText(N.JsonConvert.SerializeObject(value), NewtonsoftCompat.Serialize(value));
            AssertSameText(
                N.JsonConvert.SerializeObject(value, new N.JsonSerializerSettings { NullValueHandling = N.NullValueHandling.Ignore }),
                NewtonsoftCompat.Serialize(value, ignoreNullValues: true));
            AssertSameText(N.JsonConvert.SerializeObject(value, N.Formatting.Indented), NewtonsoftCompat.Serialize(value, indented: true));
        }

        private static void AssertSameText(string expected, string actual)
        {
            var difference = JsonTrees.Difference(JsonTrees.Parse(expected), JsonTrees.Parse(actual), "$");
            Assert.That(difference, Is.Null, $"Newtonsoft: {expected}\nNew:        {actual}");
        }
    }
}

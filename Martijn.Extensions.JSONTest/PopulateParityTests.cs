using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Martijn.Extensions.JSON.Compat;
using NUnit.Framework;
using ParityTesting;
using N = Newtonsoft.Json;
using NL = Newtonsoft.Json.Linq;
using S = System.Text.Json.Serialization;

namespace Martijn.Extensions.JSONTest
{
    public interface IHasKind
    {
        [N.JsonIgnore]
        [S.JsonIgnore]
        string? Kind { get; set; }

        [N.JsonProperty("label_from_interface")]
        [S.JsonPropertyName("label_from_interface")]
        string? Label { get; set; }
    }

    public class WithInterface : IHasKind
    {
        public string? Kind { get; set; } = "fixed";
        public string? Label { get; set; }
        public string? Other { get; set; }
    }

    public class OldWriteOnlyDefault : N.JsonConverter<Defaulted>
    {
        public override bool CanWrite => false;

        public override Defaulted ReadJson(N.JsonReader reader, Type objectType, Defaulted? existingValue, bool hasExistingValue, N.JsonSerializer serializer)
        {
            NL.JToken.ReadFrom(reader);
            return new Defaulted { Name = "from converter" };
        }

        public override void WriteJson(N.JsonWriter writer, Defaulted? value, N.JsonSerializer serializer) => throw new NotImplementedException();
    }

    public class NewWriteOnlyDefault : S.JsonConverter<Defaulted>
    {
        // Newtonsoft passes JSON null to converters.
        public override bool HandleNull => true;

        public override Defaulted Read(ref System.Text.Json.Utf8JsonReader reader, Type typeToConvert, System.Text.Json.JsonSerializerOptions options)
        {
            reader.Skip();
            return new Defaulted { Name = "from converter" };
        }

        public override void Write(System.Text.Json.Utf8JsonWriter writer, Defaulted value, System.Text.Json.JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue();
                return;
            }
            NewtonsoftPopulate.WriteDefault(writer, value, options);
        }
    }

    [N.JsonConverter(typeof(OldWriteOnlyDefault))]
    [S.JsonConverter(typeof(NewWriteOnlyDefault))]
    public class Defaulted
    {
        [N.JsonProperty("name")]
        [S.JsonPropertyName("name")]
        public string? Name { get; set; }

        [N.JsonProperty("skip", NullValueHandling = N.NullValueHandling.Ignore)]
        [S.JsonPropertyName("skip")]
        [NullValueHandling(NullHandling.Ignore)]
        public string? Skip { get; set; }

        public Animal? Pet { get; set; }

        public List<Animal> Pets { get; set; } = new();

        [N.JsonIgnore]
        [S.JsonIgnore]
        public string Hidden => "hidden";

        public string Computed => "computed";
    }

    public class HoldsDefaulted
    {
        public Defaulted? Item { get; set; }
        public List<Defaulted>? Items { get; set; }
    }

    /// <summary>
    /// The compat layer's stand-ins for Newtonsoft APIs that ApiLibs' custom converters call:
    /// JsonSerializer.Populate over a JTokenReader, JToken.ToObject&lt;string&gt;(), interface
    /// attributes, and default writing for a converter with CanWrite => false.
    /// </summary>
    [Category("NoInternet")]
    public class PopulateParityTests
    {
        private static readonly Type[] HolderTypes =
        {
            typeof(string), typeof(int), typeof(int?), typeof(long), typeof(short), typeof(double), typeof(decimal), typeof(bool),
            typeof(DateTime), typeof(DateTime?), typeof(DateTimeOffset), typeof(DateTimeOffset?), typeof(Guid), typeof(Uri),
            typeof(Colour), typeof(object), typeof(Nested), typeof(List<int>), typeof(List<string>), typeof(Dictionary<string, object>),
        };

        public static IEnumerable<TestCaseData> Holders() => HolderTypes.Select(t => new TestCaseData(t).SetName($"Populate Holder<{Names.Short(t)}>"));

        private static void OldPopulate(string json, object target)
        {
            var token = NL.JToken.ReadFrom(new N.JsonTextReader(new StringReader(json)));
            N.JsonSerializer.CreateDefault().Populate(token.CreateReader(), target);
        }

        private static void NewPopulate(string json, object target)
        {
            using var document = System.Text.Json.JsonDocument.Parse(json);
            NewtonsoftPopulate.Populate(document.RootElement.Clone(), target);
        }

        private static string? ComparePopulate(string json, Func<object> create)
        {
            var oldTarget = create();
            var newTarget = create();
            var (_, oldError) = Parity.Try(() => { OldPopulate(json, oldTarget); return null; });
            var (_, newError) = Parity.Try(() => { NewPopulate(json, newTarget); return null; });
            if (oldError != null || newError != null)
            {
                return oldError != null && newError != null ? null
                    : $"Newtonsoft: {oldError?.Message ?? "ok"} / new: {newError?.Message ?? "ok"}";
            }
            return DeepComparer.Compare(oldTarget, newTarget);
        }

        [TestCaseSource(nameof(Holders))]
        public void PopulateHolder(Type type)
        {
            var holder = typeof(Holder<>).MakeGenericType(type);
            var failures = new List<string>();
            foreach (var literal in PrimitiveReadParityTests.Literals)
            {
                Parity.Collect(failures, literal, ComparePopulate("{\"V\":" + literal + "}", () => Activator.CreateInstance(holder)!));
            }
            Assert.That(failures, Is.Empty, string.Join("\n", failures));
        }

        public static IEnumerable<TestCaseData> ObjectCases() => new (Func<object> create, string json)[]
        {
            (() => new Attributed(), "{\"the_name\":\"n\",\"count\":null,\"label\":\"2020-01-02T03:04:05Z\",\"Tags\":[\"x\"]}"),
            (() => new Prefilled(), "{\"Main\":{\"Size\":14},\"Fixed\":[\"json\"],\"Settable\":[\"json\"],\"Missing\":{\"Font\":\"x\"}}"),
            (() => new Prefilled(), "{\"Main\":null,\"Settable\":null}"),
            (() => new WithInterface(), "{\"Kind\":\"from json\",\"label_from_interface\":\"l\",\"Label\":\"plain\",\"Other\":\"o\"}"),
            (() => new WithCasts(), "{\"Edited\":true,\"Many\":[1.5,false]}"),
            (() => new Zoo(), "{\"Star\":{\"Name\":\"s\"},\"Things\":[\"2020-01-02T03:04:05+02:00\",1.25,true]}"),
            (() => new HoldsDefaulted(), "{\"Item\":{\"name\":\"ignored\"},\"Items\":[{},null]}"),
            (() => new List<Nested>(), "[{\"Id\":1},{\"Id\":\"2\"}]"),
            (() => new Dictionary<string, object>(), "{\"a\":1,\"b\":\"2020-01-02T03:04:05Z\"}"),
            (() => new Nested(), "[]"),
            (() => new Nested(), "\"text\""),
            (() => new Nested(), "null"),
        }.Select((c, i) => new TestCaseData(c.create, c.json).SetName($"Populate object #{i}"));

        [TestCaseSource(nameof(ObjectCases))]
        public void PopulateObject(Func<object> create, string json)
        {
            Assert.That(ComparePopulate(json, create), Is.Null, json);
        }

        [Test]
        public void ValueAsStringMatchesToObjectString()
        {
            var failures = new List<string>();
            foreach (var literal in PrimitiveReadParityTests.Literals)
            {
                var (oldValue, oldError) = Parity.Try(() => NL.JToken.ReadFrom(new N.JsonTextReader(new StringReader(literal))).ToObject<string>());
                var (newValue, newError) = Parity.Try(() =>
                {
                    using var document = System.Text.Json.JsonDocument.Parse(literal);
                    return NewtonsoftPopulate.ValueAsString(document.RootElement);
                });
                if ((oldError == null) != (newError == null) || (oldError == null && !Equals(oldValue, newValue)))
                {
                    failures.Add($"{literal}: {oldValue ?? oldError?.GetType().Name} vs {newValue ?? newError?.GetType().Name}");
                }
            }
            Assert.That(failures, Is.Empty, string.Join("\n", failures));
        }

        [Test]
        public void InterfaceAttributesApply()
        {
            Assert.That(Parity.CompareRead("{\"Kind\":\"json\",\"label_from_interface\":\"l\",\"Other\":\"o\"}", typeof(WithInterface)), Is.Null);
            var value = new WithInterface { Label = "l", Other = "o" };
            Assert.That(NewtonsoftCompat.Serialize(value), Is.EqualTo(N.JsonConvert.SerializeObject(value)));
        }

        [Test]
        public void WriteDefaultMatchesCanWriteFalse()
        {
            var values = new object[]
            {
                new Defaulted { Name = "n", Pet = new Dog { Name = "d", Barks = true }, Pets = { new Dog(), new Animal() } },
                new Defaulted(),
                new HoldsDefaulted { Item = new Defaulted { Skip = "s" }, Items = new List<Defaulted> { new Defaulted(), null! } },
            };
            foreach (var value in values)
            {
                foreach (var ignoreNull in new[] { false, true })
                {
                    var expected = N.JsonConvert.SerializeObject(value, new N.JsonSerializerSettings { NullValueHandling = ignoreNull ? N.NullValueHandling.Ignore : N.NullValueHandling.Include });
                    var actual = NewtonsoftCompat.Serialize(value, ignoreNullValues: ignoreNull);
                    Assert.That(JsonTrees.Difference(JsonTrees.Parse(expected), JsonTrees.Parse(actual), "$"), Is.Null, $"{expected}\n{actual}");
                }
            }
        }

        [Test]
        public void ConverterReadsWithoutWriting()
        {
            Assert.That(Parity.CompareRead("{\"Item\":{\"name\":\"x\"},\"Items\":[{\"name\":\"y\"}]}", typeof(HoldsDefaulted)), Is.Null);
        }
    }
}

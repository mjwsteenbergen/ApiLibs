using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using ApiLibs.JSON.Serialization;
using NUnit.Framework;

namespace ApiLibs.JSONTest.Serialization
{
    [Category("NoInternet")]
    public class JsonCompatTests
    {
        private class AnimalConverter : JsonConverter<Animal>
        {
            public override Animal? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                using var document = JsonDocument.ParseValue(ref reader);
                var json = document.RootElement.Clone();
                Animal result = JsonCompat.ValueAsString(json.GetProperty("kind")) switch
                {
                    "dog" => new Dog(),
                    _ => new Cat(),
                };
                JsonCompat.Populate(json, result);
                return result;
            }

            public override void Write(Utf8JsonWriter writer, Animal value, JsonSerializerOptions options) =>
                JsonCompat.WriteDefault(writer, value, options);
        }

        [JsonConverter(typeof(AnimalConverter))]
        private abstract class Animal
        {
            [JsonPropertyName("kind")]
            public string Kind { get; set; } = "";

            [JsonPropertyName("name")]
            public string? Name { get; set; }
        }

        private class Dog : Animal
        {
            [JsonPropertyName("goodBoy")]
            public bool GoodBoy { get; set; }
        }

        private class Cat : Animal
        {
            [JsonPropertyName("lives")]
            public int Lives { get; set; } = 9;
        }

        [Test]
        public void AConverterCanPickASubclassAndPopulateIt()
        {
            var animals = JsonCompat.Deserialize<List<Animal>>(
                """[{"kind":"dog","name":"Rex","goodBoy":true},{"kind":"cat","name":"Tom"}]""");

            Assert.That(animals[0], Is.TypeOf<Dog>());
            Assert.That(((Dog)animals[0]).GoodBoy, Is.True);
            Assert.That(animals[0].Name, Is.EqualTo("Rex"));
            Assert.That(animals[1], Is.TypeOf<Cat>());
            Assert.That(((Cat)animals[1]).Lives, Is.EqualTo(9), "members the JSON leaves out keep their value");
        }

        [Test]
        public void ASubclassIsReadThroughTheBaseClassConverter()
        {
            var dog = JsonCompat.Deserialize<Dog>("""{"kind":"dog","goodBoy":true}""");

            Assert.That(dog.GoodBoy, Is.True);
        }

        [Test]
        public void WriteDefaultWritesTheRuntimeMembers()
        {
            var json = JsonCompat.Serialize(new Dog { Kind = "dog", Name = "Rex", GoodBoy = true });

            Assert.That(json, Does.Contain("\"goodBoy\":true"));
            Assert.That(json, Does.Contain("\"name\":\"Rex\""));
        }

        private class WithNull
        {
            [JsonPropertyName("a")]
            public string? A { get; set; }

            [JsonPropertyName("b")]
            public string? B { get; set; }
        }

        [Test]
        public void SerializeCanSkipNulls()
        {
            Assert.That(JsonCompat.Serialize(new WithNull { A = "x" }, ignoreNullValues: true), Is.EqualTo("""{"a":"x"}"""));
            Assert.That(JsonCompat.Serialize(new WithNull { A = "x" }), Is.EqualTo("""{"a":"x","b":null}"""));
        }
    }
}

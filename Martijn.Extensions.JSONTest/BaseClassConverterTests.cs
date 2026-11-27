using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Martijn.Extensions.JSON.Compat;
using NUnit.Framework;

namespace Martijn.Extensions.JSONTest
{
    /// <summary>
    /// A converter on a base class applies to that class only, as in System.Text.Json. A converter
    /// that picks a subclass can then read it with JsonSerializer without being called again.
    /// </summary>
    public class BaseClassConverterTests
    {
        public class AnimalConverter : JsonConverter<Animal>
        {
            public override Animal? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                using var document = JsonDocument.ParseValue(ref reader);
                var json = document.RootElement;
                Type type = json.GetProperty("kind").GetString() == "dog" ? typeof(Dog) : typeof(Cat);
                return (Animal?)json.Deserialize(type, options);
            }

            public override void Write(Utf8JsonWriter writer, Animal value, JsonSerializerOptions options) =>
                JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }

        [JsonConverter(typeof(AnimalConverter))]
        public abstract class Animal
        {
            [JsonPropertyName("kind")]
            public string Kind { get; set; } = "";
        }

        public class Dog : Animal
        {
            [JsonPropertyName("goodBoy")]
            public bool GoodBoy { get; set; }
        }

        public class Cat : Animal
        {
            [JsonPropertyName("lives")]
            public int Lives { get; set; }
        }

        public class Zoo
        {
            [JsonPropertyName("star")]
            public Animal? Star { get; set; }

            [JsonPropertyName("dog")]
            public Dog? Dog { get; set; }
        }

        [Test]
        public void AConverterCanReadTheSubclassItPicked()
        {
            var animals = NewtonsoftCompat.Deserialize<List<Animal>>("""[{"kind":"dog","goodBoy":true},{"kind":"cat","lives":7}]""");

            Assert.That(animals[0], Is.TypeOf<Dog>());
            Assert.That(((Dog)animals[0]).GoodBoy, Is.True);
            Assert.That(animals[1], Is.TypeOf<Cat>());
            Assert.That(((Cat)animals[1]).Lives, Is.EqualTo(7));
        }

        [Test]
        public void ASubclassIsReadAsItself()
        {
            var zoo = NewtonsoftCompat.Deserialize<Zoo>("""{"star":{"kind":"cat","lives":3},"dog":{"goodBoy":true}}""");

            Assert.That(zoo.Star, Is.TypeOf<Cat>());
            Assert.That(zoo.Dog!.GoodBoy, Is.True, "no 'kind', so this only works if Dog does not go through AnimalConverter");
        }

        [Test]
        public void TheConverterWritesTheRuntimeType()
        {
            var json = NewtonsoftCompat.Serialize(new Zoo { Star = new Dog { Kind = "dog", GoodBoy = true } }, ignoreNullValues: true);

            Assert.That(json, Is.EqualTo("""{"star":{"goodBoy":true,"kind":"dog"}}"""));
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Martijn.Extensions.JSON.Compat
{
    /// <summary>
    /// Lists, arrays and string-keyed dictionaries, read and written element by element so that
    /// Newtonsoft's element rules apply: an empty string becomes null for nullable element types,
    /// and every element is written using its runtime type.
    /// </summary>
    internal sealed class CompatCollectionConverterFactory : JsonConverterFactory
    {
        private static readonly Type[] ListLike =
        {
            typeof(List<>), typeof(IEnumerable<>), typeof(IList<>), typeof(ICollection<>),
            typeof(IReadOnlyList<>), typeof(IReadOnlyCollection<>),
        };

        private static readonly Type[] DictionaryLike =
        {
            typeof(Dictionary<,>), typeof(IDictionary<,>), typeof(IReadOnlyDictionary<,>),
        };

        public override bool CanConvert(Type typeToConvert)
        {
            if (typeToConvert.IsArray)
            {
                // byte[] is written as base64, like Newtonsoft does.
                return typeToConvert.GetArrayRank() == 1 && typeToConvert != typeof(byte[]);
            }

            if (!typeToConvert.IsGenericType)
            {
                return false;
            }

            var definition = typeToConvert.GetGenericTypeDefinition();
            if (ListLike.Contains(definition))
            {
                return true;
            }

            return DictionaryLike.Contains(definition) && typeToConvert.GetGenericArguments()[0] == typeof(string);
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            if (typeToConvert.IsArray)
            {
                return (JsonConverter)Activator.CreateInstance(typeof(ArrayConverter<>).MakeGenericType(typeToConvert.GetElementType()!))!;
            }

            var args = typeToConvert.GetGenericArguments();
            if (args.Length == 2)
            {
                return (JsonConverter)Activator.CreateInstance(typeof(DictionaryConverter<,>).MakeGenericType(typeToConvert, args[1]))!;
            }

            return (JsonConverter)Activator.CreateInstance(typeof(ListConverter<,>).MakeGenericType(typeToConvert, args[0]))!;
        }

        internal static bool CoercesEmptyString(Type type)
            => (!type.IsValueType || Nullable.GetUnderlyingType(type) != null) && type != typeof(string) && type != typeof(object);

        internal static TElement? ReadElement<TElement>(ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String && CoercesEmptyString(typeof(TElement)) && reader.ValueTextEquals(ReadOnlySpan<byte>.Empty))
            {
                return default;
            }

            if (CastOperators.IsPrimitiveToken(reader.TokenType) && CastOperators.Has(typeof(TElement)))
            {
                return (TElement?)CastOperators.Read(ref reader, typeof(TElement));
            }

            return JsonSerializer.Deserialize<TElement>(ref reader, options);
        }

        internal static void WriteElement(Utf8JsonWriter writer, object? value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue();
            }
            else
            {
                JsonSerializer.Serialize(writer, value, value.GetType(), options);
            }
        }

        private static List<TElement> ReadList<TElement>(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException($"Cannot deserialize {reader.TokenType} into {typeToConvert}.");
            }

            var list = new List<TElement>();
            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
            {
                list.Add(ReadElement<TElement>(ref reader, options)!);
            }
            return list;
        }

        private sealed class ListConverter<TCollection, TElement> : JsonConverter<TCollection>
        {
            public override TCollection? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.String && reader.ValueTextEquals(ReadOnlySpan<byte>.Empty))
                {
                    return default;
                }
                return (TCollection)(object)ReadList<TElement>(ref reader, typeToConvert, options);
            }

            public override void Write(Utf8JsonWriter writer, TCollection value, JsonSerializerOptions options)
            {
                writer.WriteStartArray();
                foreach (var item in (IEnumerable)value!)
                {
                    WriteElement(writer, item, options);
                }
                writer.WriteEndArray();
            }
        }

        private sealed class ArrayConverter<TElement> : JsonConverter<TElement[]>
        {
            public override TElement[]? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.String && reader.ValueTextEquals(ReadOnlySpan<byte>.Empty))
                {
                    return null;
                }
                return ReadList<TElement>(ref reader, typeToConvert, options).ToArray();
            }

            public override void Write(Utf8JsonWriter writer, TElement[] value, JsonSerializerOptions options)
            {
                writer.WriteStartArray();
                foreach (var item in value)
                {
                    WriteElement(writer, item, options);
                }
                writer.WriteEndArray();
            }
        }

        private sealed class DictionaryConverter<TDictionary, TValue> : JsonConverter<TDictionary>
        {
            public override TDictionary? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.String && reader.ValueTextEquals(ReadOnlySpan<byte>.Empty))
                {
                    return default;
                }

                if (reader.TokenType != JsonTokenType.StartObject)
                {
                    throw new JsonException($"Cannot deserialize {reader.TokenType} into {typeToConvert}.");
                }

                var dictionary = new Dictionary<string, TValue>();
                while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
                {
                    var key = reader.GetString()!;
                    reader.Read();
                    // Newtonsoft overwrites duplicate keys.
                    dictionary[key] = ReadElement<TValue>(ref reader, options)!;
                }
                return (TDictionary)(object)dictionary;
            }

            public override void Write(Utf8JsonWriter writer, TDictionary value, JsonSerializerOptions options)
            {
                writer.WriteStartObject();
                foreach (DictionaryEntry entry in (IDictionary)value!)
                {
                    writer.WritePropertyName((string)entry.Key);
                    WriteElement(writer, entry.Value, options);
                }
                writer.WriteEndObject();
            }
        }
    }
}

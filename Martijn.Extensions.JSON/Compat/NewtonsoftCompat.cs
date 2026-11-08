using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace Martijn.Extensions.JSON.Compat
{
    /// <summary>
    /// System.Text.Json set up to read and write JSON the way Newtonsoft.Json 12 does with its
    /// default settings. This is what lets ApiLibs.JSON and Martijn.Extensions.JSON keep parity
    /// with the Newtonsoft-based packages. Differences that cannot be reproduced are listed in
    /// QUIRKS.md at the root of the repository.
    /// </summary>
    internal static class NewtonsoftCompat
    {
        /// <summary>Equivalent of <c>new JsonSerializerSettings()</c>: nulls are written.</summary>
        public static JsonSerializerOptions Options { get; } = Create(ignoreNullValues: false, indented: false);

        /// <summary>Equivalent of <c>NullValueHandling = NullValueHandling.Ignore</c>.</summary>
        public static JsonSerializerOptions OptionsIgnoreNull { get; } = Create(ignoreNullValues: true, indented: false);

        /// <summary>Equivalent of <c>Formatting.Indented</c>.</summary>
        public static JsonSerializerOptions OptionsIndented { get; } = Create(ignoreNullValues: false, indented: true);

        /// <summary>
        /// Reading as Newtonsoft does through a JTokenReader, which is what custom converters that
        /// called <c>serializer.Populate(jToken.CreateReader(), target)</c> got.
        /// </summary>
        public static JsonSerializerOptions TokenReaderOptions { get; } = Create(ignoreNullValues: false, indented: false, tokenReaderMode: true);

        private static JsonSerializerOptions Create(bool ignoreNullValues, bool indented, bool tokenReaderMode = false)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                AllowTrailingCommas = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                IncludeFields = true,
                WriteIndented = indented,
                DefaultIgnoreCondition = ignoreNullValues ? JsonIgnoreCondition.WhenWritingNull : JsonIgnoreCondition.Never,
                Encoder = NewtonsoftJavaScriptEncoder.Instance,
                MaxDepth = 64,
                // Newtonsoft fills an existing object behind a property instead of replacing it
                // (ObjectCreationHandling.Auto), also when the property has no public setter.
                PreferredObjectCreationHandling = JsonObjectCreationHandling.Populate,
                TypeInfoResolver = new DefaultJsonTypeInfoResolver
                {
                    Modifiers = { CompatTypeInfoModifier.Modify }
                }
            };

            foreach (var converter in CompatConverters.All(tokenReaderMode))
            {
                options.Converters.Add(converter);
            }

            options.MakeReadOnly();
            return options;
        }

        /// <summary>Equivalent of <c>JsonConvert.DeserializeObject&lt;T&gt;(text)</c>.</summary>
        public static T Deserialize<T>(string text) => (T)Deserialize(text, typeof(T))!;

        /// <summary>Equivalent of <c>JsonConvert.DeserializeObject(text, type)</c>.</summary>
        public static object? Deserialize(string text, Type type)
        {
            if (text == null)
            {
                throw new ArgumentNullException("value");
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                // Newtonsoft returns null for an empty document, unless the type cannot be null.
                if (type.IsValueType && Nullable.GetUnderlyingType(type) == null)
                {
                    throw new JsonException($"No JSON content found and type '{type}' is not nullable.");
                }
                return null;
            }

            if (IsEmptyStringDocument(text) && type != typeof(string) && type != typeof(object)
                && (!type.IsValueType || Nullable.GetUnderlyingType(type) != null))
            {
                // CoerceEmptyStringToNull also applies to the root value.
                return null;
            }

            if (CastOperators.Has(type))
            {
                var reader = new Utf8JsonReader(System.Text.Encoding.UTF8.GetBytes(text), new JsonReaderOptions { AllowTrailingCommas = true, CommentHandling = JsonCommentHandling.Skip });
                reader.Read();
                if (CastOperators.IsPrimitiveToken(reader.TokenType))
                {
                    return CastOperators.Read(ref reader, type);
                }
            }

            return JsonSerializer.Deserialize(text, type, Options);
        }

        /// <summary>Deserializes a JSON value with the same root rules as <see cref="Deserialize(string, Type)"/>.</summary>
        public static object? DeserializeElement(JsonElement element, Type type, JsonSerializerOptions options)
        {
            if (element.ValueKind == JsonValueKind.String && element.ValueEquals("") && type != typeof(string) && type != typeof(object)
                && (!type.IsValueType || Nullable.GetUnderlyingType(type) != null))
            {
                return null;
            }

            if (CastOperators.Has(type) && element.ValueKind is JsonValueKind.String or JsonValueKind.Number or JsonValueKind.True or JsonValueKind.False)
            {
                var reader = new Utf8JsonReader(System.Text.Encoding.UTF8.GetBytes(element.GetRawText()));
                reader.Read();
                return CastOperators.Read(ref reader, type);
            }

            return JsonSerializer.Deserialize(element, type, options);
        }

        private static bool IsEmptyStringDocument(string text)
        {
            var trimmed = text.Trim();
            return trimmed == "\"\"" || trimmed == "''";
        }

        /// <summary>Equivalent of <c>JsonConvert.SerializeObject(value)</c> with the matching settings.</summary>
        public static string Serialize(object? value, bool ignoreNullValues = false, bool indented = false)
        {
            var options = indented ? OptionsIndented : ignoreNullValues ? OptionsIgnoreNull : Options;
            if (indented && ignoreNullValues)
            {
                throw new NotSupportedException("Indented output that skips nulls is not used anywhere.");
            }

            // Newtonsoft always serializes the runtime type.
            return JsonSerializer.Serialize(value, value?.GetType() ?? typeof(object), options);
        }
    }
}

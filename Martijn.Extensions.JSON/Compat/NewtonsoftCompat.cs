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

        private static JsonSerializerOptions Create(bool ignoreNullValues, bool indented)
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
                TypeInfoResolver = new DefaultJsonTypeInfoResolver
                {
                    Modifiers = { CompatTypeInfoModifier.Modify }
                }
            };

            foreach (var converter in CompatConverters.All())
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

            return JsonSerializer.Deserialize(text, type, Options);
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

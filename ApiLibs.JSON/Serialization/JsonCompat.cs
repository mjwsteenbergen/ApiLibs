using System;
using System.Text.Json;
using Martijn.Extensions.JSON.Compat;

namespace ApiLibs.JSON.Serialization
{
    /// <summary>
    /// The Newtonsoft calls that code built on ApiLibs made directly (<c>JsonConvert</c>,
    /// <c>JsonSerializer.Populate</c>, <c>JToken.ToObject&lt;string&gt;()</c>), backed by the same
    /// compatibility layer ApiLibs.JSON uses for its own requests, so a library ported along with
    /// ApiLibs reads and writes JSON the way its requests do.
    /// </summary>
    public static class JsonCompat
    {
        /// <summary><c>JsonConvert.SerializeObject(value)</c>, optionally with <c>NullValueHandling.Ignore</c> or <c>Formatting.Indented</c>.</summary>
        public static string Serialize(object value, bool ignoreNullValues = false, bool indented = false) =>
            NewtonsoftCompat.Serialize(value, ignoreNullValues, indented);

        /// <summary><c>JsonConvert.DeserializeObject&lt;T&gt;(text)</c>.</summary>
        public static T Deserialize<T>(string text) => NewtonsoftCompat.Deserialize<T>(text);

        /// <summary><c>JsonConvert.DeserializeObject(text, type)</c>.</summary>
        public static object Deserialize(string text, Type type) => NewtonsoftCompat.Deserialize(text, type);

        /// <summary>
        /// <c>serializer.Populate(jToken.CreateReader(), target)</c>: applies the JSON's members to
        /// <paramref name="target"/> without going through the converter on its type, so a
        /// converter can create a subclass and fill it in. Members the JSON leaves out keep their value.
        /// </summary>
        public static void Populate(JsonElement json, object target) => NewtonsoftPopulate.Populate(json, target);

        /// <summary><c>jToken.ToObject&lt;string&gt;()</c>.</summary>
        public static string ValueAsString(JsonElement token) => NewtonsoftPopulate.ValueAsString(token);

        /// <summary>
        /// Writes <paramref name="value"/> member by member, ignoring the converter on its type: what
        /// Newtonsoft does for a converter with <c>CanWrite => false</c>.
        /// </summary>
        public static void WriteDefault(Utf8JsonWriter writer, object value, JsonSerializerOptions options) =>
            NewtonsoftPopulate.WriteDefault(writer, value, options);
    }
}

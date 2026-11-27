using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Martijn.Extensions.JSON.Compat
{
    /// <summary>
    /// Pieces of Newtonsoft's API that custom converters used: <c>JsonSerializer.Populate</c>,
    /// writing an object without its converter (a converter with <c>CanWrite => false</c>), and
    /// <c>JToken.ToObject&lt;string&gt;()</c>.
    /// </summary>
    internal static class NewtonsoftPopulate
    {
        /// <summary>
        /// Equivalent of <c>serializer.Populate(jToken.CreateReader(), target)</c>: the JSON members
        /// are applied to <paramref name="target"/>; members the JSON does not mention keep their value.
        /// Values are read the way a JTokenReader presents them (see NewtonsoftCompat.TokenReaderOptions).
        /// </summary>
        public static void Populate(JsonElement json, object target)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (json.ValueKind == JsonValueKind.Array && target is IList list && !list.IsFixedSize && !list.IsReadOnly)
            {
                var elementType = ElementType(target.GetType()) ?? typeof(object);
                foreach (var item in json.EnumerateArray())
                {
                    list.Add(NewtonsoftCompat.DeserializeElement(item, elementType, NewtonsoftCompat.TokenReaderOptions));
                }
                return;
            }

            if (json.ValueKind != JsonValueKind.Object)
            {
                throw new JsonException($"Unexpected initial token '{json.ValueKind}' when populating object. Expected JSON object or array.");
            }

            if (target is IDictionary dictionary)
            {
                var valueType = target.GetType().IsGenericType ? target.GetType().GetGenericArguments().Last() : typeof(object);
                foreach (var property in json.EnumerateObject())
                {
                    dictionary[property.Name] = NewtonsoftCompat.DeserializeElement(property.Value, valueType, NewtonsoftCompat.TokenReaderOptions);
                }
                return;
            }

            var members = Members(target.GetType());
            foreach (var property in json.EnumerateObject())
            {
                var member = members.FirstOrDefault(m => m.Name == property.Name)
                             ?? members.FirstOrDefault(m => string.Equals(m.Name, property.Name, StringComparison.OrdinalIgnoreCase));
                if (member == null)
                {
                    continue;
                }

                var token = property.Value;
                object? current = null;
                bool useExisting = false;
                if ((token.ValueKind is JsonValueKind.Object or JsonValueKind.Array || member.Converter != null) && member.CanRead)
                {
                    current = member.Get(target);
                    useExisting = current != null && !current.GetType().IsValueType && current is not Array
                                  && !(current is IList { IsReadOnly: true });
                }

                if (!member.CanWrite && !useExisting)
                {
                    continue;
                }

                if (member.IgnoreNull && token.ValueKind == JsonValueKind.Null)
                {
                    continue;
                }

                object? value;
                if (member.Converter != null)
                {
                    value = ReadWithConverter(token, member.Type, member.Converter);
                }
                else if (HasTypeConverter(member.Type))
                {
                    // The type's own converter reads it (Newtonsoft would hand it the existing value).
                    value = NewtonsoftCompat.DeserializeElement(token, member.Type, NewtonsoftCompat.TokenReaderOptions);
                }
                else if (useExisting && token.ValueKind == JsonValueKind.Object && current is not IList)
                {
                    Populate(token, current!);
                    value = current;
                }
                else if (useExisting && token.ValueKind == JsonValueKind.Array && current is IList)
                {
                    Populate(token, current!);
                    value = current;
                }
                else
                {
                    value = NewtonsoftCompat.DeserializeElement(token, member.Type, NewtonsoftCompat.TokenReaderOptions);
                }

                if ((!useExisting || !ReferenceEquals(value, current)) && member.CanWrite)
                {
                    member.Set(target, value);
                }
            }
        }

        /// <summary>
        /// Writes <paramref name="value"/> as a JSON object member by member, ignoring any converter
        /// on its type: what Newtonsoft does when the type's converter has <c>CanWrite => false</c>.
        /// </summary>
        public static void WriteDefault(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            foreach (var member in Members(value.GetType()).Where(m => m.CanRead))
            {
                object? memberValue;
                try
                {
                    memberValue = member.Get(value);
                }
                catch (TargetInvocationException e)
                {
                    throw new JsonException($"Error getting value from '{member.Name}' on '{value.GetType()}'.", e.InnerException);
                }

                if (memberValue == null)
                {
                    bool skip = member.NullHandling == NullHandling.Ignore
                                || (member.NullHandling != NullHandling.Include && options.DefaultIgnoreCondition == JsonIgnoreCondition.WhenWritingNull);
                    if (skip)
                    {
                        continue;
                    }
                }

                writer.WritePropertyName(member.Name);
                if (memberValue == null)
                {
                    writer.WriteNullValue();
                }
                else if (member.Converter != null)
                {
                    JsonSerializer.Serialize(writer, memberValue, member.Type, WithConverter(options, member.Converter));
                }
                else
                {
                    JsonSerializer.Serialize(writer, memberValue, memberValue.GetType(), options);
                }
            }
            writer.WriteEndObject();
        }

        /// <summary>
        /// <c>JToken.ToObject&lt;string&gt;()</c> and <c>JToken.Value&lt;string&gt;()</c> on a token that
        /// Newtonsoft parsed with its default settings: ISO date strings had become DateTime and
        /// come back in the invariant "G" format, booleans as "True"/"False".
        /// </summary>
        public static string? ValueAsString(JsonElement token)
        {
            switch (token.ValueKind)
            {
                case JsonValueKind.String:
                    var s = token.GetString()!;
                    return NewtonsoftDates.TryParseDateTime(s, out var date) ? Convert.ToString(date, CultureInfo.InvariantCulture) : s;
                case JsonValueKind.Number:
                    var raw = token.GetRawText();
                    if (raw.IndexOfAny(new[] { '.', 'e', 'E' }) == -1)
                    {
                        return long.TryParse(raw, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out var l)
                            ? l.ToString(CultureInfo.InvariantCulture)
                            : BigInteger.Parse(raw, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture);
                    }
                    return Convert.ToString(double.Parse(raw, NumberStyles.Float, CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
                case JsonValueKind.True:
                    return "True";
                case JsonValueKind.False:
                    return "False";
                case JsonValueKind.Null:
                case JsonValueKind.Undefined:
                    return null;
                default:
                    throw new InvalidCastException($"Cannot cast {token.ValueKind} to string.");
            }
        }

        private static bool HasTypeConverter(Type type) => type.GetCustomAttribute<JsonConverterAttribute>(true) != null;

        private static object? ReadWithConverter(JsonElement token, Type type, JsonConverter converter)
        {
            return JsonSerializer.Deserialize(token.GetRawText(), type, WithConverter(NewtonsoftCompat.TokenReaderOptions, converter));
        }

        private static readonly ConcurrentDictionary<(JsonSerializerOptions, Type), JsonSerializerOptions> ConverterOptions = new();

        private static JsonSerializerOptions WithConverter(JsonSerializerOptions options, JsonConverter converter)
        {
            return ConverterOptions.GetOrAdd((options, converter.GetType()), key =>
            {
                var copy = new JsonSerializerOptions(key.Item1);
                copy.Converters.Insert(0, converter);
                copy.MakeReadOnly();
                return copy;
            });
        }

        private static Type? ElementType(Type type)
        {
            var enumerable = type.GetInterfaces().Concat(new[] { type })
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));
            return enumerable?.GetGenericArguments()[0];
        }

        internal sealed class Member
        {
            public string Name = "";
            public Type Type = typeof(object);
            public MemberInfo Info = null!;
            public bool CanRead;
            public bool CanWrite;
            public NullHandling? NullHandling;
            public bool IgnoreNull => NullHandling == Compat.NullHandling.Ignore;
            public JsonConverter? Converter;

            public object? Get(object target) => Info is PropertyInfo p ? p.GetValue(target) : ((FieldInfo)Info).GetValue(target);

            public void Set(object target, object? value)
            {
                if (Info is PropertyInfo p)
                {
                    p.SetValue(target, value);
                }
                else
                {
                    ((FieldInfo)Info).SetValue(target, value);
                }
            }
        }

        private static readonly ConcurrentDictionary<Type, List<Member>> MemberCache = new();

        /// <summary>The members Newtonsoft's DefaultContractResolver serializes, with their JSON names.</summary>
        internal static List<Member> Members(Type type) => MemberCache.GetOrAdd(type, t =>
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var result = new List<Member>();
            var seen = new HashSet<string>();
            for (var current = t; current != null && current != typeof(object); current = current.BaseType)
            {
                foreach (var info in current.GetMembers(flags | BindingFlags.DeclaredOnly))
                {
                    if (info is not PropertyInfo and not FieldInfo || info.Name.Contains('<') || !seen.Add(info.Name))
                    {
                        continue;
                    }
                    if (info is PropertyInfo { } pi && pi.GetIndexParameters().Length > 0)
                    {
                        continue;
                    }
                    if (info.IsDefined(typeof(JsonIgnoreAttribute), true) || InterfaceAttribute<JsonIgnoreAttribute>(info) != null)
                    {
                        continue;
                    }

                    var nameAttribute = info.GetCustomAttribute<JsonPropertyNameAttribute>(true) ?? InterfaceAttribute<JsonPropertyNameAttribute>(info);
                    bool attributed = nameAttribute != null;
                    var member = new Member { Info = info, Name = nameAttribute?.Name ?? info.Name };

                    if (info is PropertyInfo property)
                    {
                        bool publicGet = property.GetMethod?.IsPublic ?? false;
                        bool publicSet = property.SetMethod?.IsPublic ?? false;
                        if (!publicGet && !publicSet && !attributed)
                        {
                            continue;
                        }
                        member.Type = property.PropertyType;
                        member.CanRead = property.GetMethod != null && (publicGet || attributed);
                        member.CanWrite = property.SetMethod != null && (publicSet || attributed);
                    }
                    else if (info is FieldInfo field)
                    {
                        if (!field.IsPublic && !attributed)
                        {
                            continue;
                        }
                        member.Type = field.FieldType;
                        member.CanRead = true;
                        member.CanWrite = !field.IsInitOnly;
                    }

                    member.NullHandling = info.GetCustomAttribute<NullValueHandlingAttribute>(true)?.Handling;
                    var converterAttribute = info.GetCustomAttribute<JsonConverterAttribute>(true);
                    if (converterAttribute?.ConverterType != null)
                    {
                        member.Converter = (JsonConverter)Activator.CreateInstance(converterAttribute.ConverterType)!;
                    }
                    result.Add(member);
                }
            }
            return result;
        });

        /// <summary>Newtonsoft also reads [JsonIgnore]/[JsonProperty] from the interface property a property implements.</summary>
        internal static T? InterfaceAttribute<T>(MemberInfo member) where T : Attribute
        {
            if (member is not PropertyInfo property || property.DeclaringType == null)
            {
                return null;
            }
            foreach (var @interface in property.DeclaringType.GetInterfaces())
            {
                var interfaceProperty = @interface.GetProperty(property.Name);
                var attribute = interfaceProperty?.GetCustomAttribute<T>(true);
                if (attribute != null)
                {
                    return attribute;
                }
            }
            return null;
        }
    }
}

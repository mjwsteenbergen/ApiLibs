using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace Martijn.Extensions.JSON.Compat
{
    /// <summary>
    /// Adjusts System.Text.Json contracts to Newtonsoft's per-property behaviour:
    /// <list type="bullet">
    /// <item>[JsonPropertyName] (the replacement of [JsonProperty]) lets non-public setters be used.</item>
    /// <item>[NullValueHandling(Ignore)] skips nulls when writing and JSON nulls when reading.</item>
    /// <item>An empty string read into a nullable non-string property becomes null.</item>
    /// <item>Values are written using their runtime type, also inside collections.</item>
    /// </list>
    /// </summary>
    internal static class CompatTypeInfoModifier
    {
        public static void Modify(JsonTypeInfo typeInfo)
        {
            if (typeInfo.Kind != JsonTypeInfoKind.Object)
            {
                return;
            }

            AddNonPublicMembers(typeInfo);
            RemoveIgnoredNameClashes(typeInfo);

            foreach (var property in typeInfo.Properties)
            {
                var member = property.AttributeProvider as MemberInfo;

                if (property.Set == null && member is PropertyInfo pi && pi.IsDefined(typeof(JsonPropertyNameAttribute), true))
                {
                    var setter = pi.GetSetMethod(nonPublic: true);
                    if (setter != null)
                    {
                        property.Set = (obj, value) => setter.Invoke(obj, new[] { value });
                    }
                }

                var nullHandling = member?.GetCustomAttribute<NullValueHandlingAttribute>(true)?.Handling;
                bool skipNull = nullHandling == NullHandling.Ignore;
                if (nullHandling == NullHandling.Ignore)
                {
                    property.ShouldSerialize = (_, value) => value != null;
                }
                else if (nullHandling == NullHandling.Include)
                {
                    property.ShouldSerialize = (_, _) => true;
                }

                var explicitConverter = property.CustomConverter ?? TypeLevelConverter(property.PropertyType);
                bool coerceEmptyString = explicitConverter == null && CoercesEmptyString(property.PropertyType);
                var polymorphism = explicitConverter == null ? GetPolymorphism(property.PropertyType) : Polymorphism.None;

                if (!skipNull && !coerceEmptyString && polymorphism == Polymorphism.None)
                {
                    continue;
                }

                var converterType = typeof(CompatPropertyConverter<>).MakeGenericType(property.PropertyType);
                property.CustomConverter = (JsonConverter)Activator.CreateInstance(converterType, explicitConverter, skipNull, coerceEmptyString, polymorphism)!;

                if (skipNull && property.Set != null)
                {
                    var set = property.Set;
                    property.Set = (obj, value) =>
                    {
                        if (NullSkip.Consume())
                        {
                            return;
                        }
                        set(obj, value);
                    };
                }
            }
        }

        /// <summary>
        /// Newtonsoft serializes non-public properties and fields that carry [JsonProperty];
        /// System.Text.Json only sees public ones, so add those marked [JsonPropertyName].
        /// </summary>
        private static void AddNonPublicMembers(JsonTypeInfo typeInfo)
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
            var existing = new HashSet<string>(typeInfo.Properties.Select(p => p.Name));

            for (var type = typeInfo.Type; type != null && type != typeof(object); type = type.BaseType)
            {
                foreach (var member in type.GetMembers(flags | BindingFlags.DeclaredOnly))
                {
                    var name = member.GetCustomAttribute<JsonPropertyNameAttribute>(true)?.Name;
                    if (name == null || existing.Contains(name) || member.IsDefined(typeof(JsonIgnoreAttribute), true))
                    {
                        continue;
                    }

                    JsonPropertyInfo? added = null;
                    if (member is PropertyInfo pi && pi.GetIndexParameters().Length == 0 && !(pi.GetMethod?.IsPublic ?? false))
                    {
                        added = typeInfo.CreateJsonPropertyInfo(pi.PropertyType, name);
                        var getter = pi.GetGetMethod(true);
                        var setter = pi.GetSetMethod(true);
                        if (getter != null)
                        {
                            added.Get = obj => getter.Invoke(obj, null);
                        }
                        if (setter != null)
                        {
                            added.Set = (obj, value) => setter.Invoke(obj, new[] { value });
                        }
                    }
                    else if (member is FieldInfo fi && !fi.IsPublic)
                    {
                        added = typeInfo.CreateJsonPropertyInfo(fi.FieldType, name);
                        added.Get = obj => fi.GetValue(obj);
                        if (!fi.IsInitOnly)
                        {
                            added.Set = (obj, value) => fi.SetValue(obj, value);
                        }
                    }

                    if (added != null)
                    {
                        added.AttributeProvider = member;
                        typeInfo.Properties.Add(added);
                        existing.Add(name);
                    }
                }
            }
        }

        /// <summary>
        /// [JsonIgnore] properties stay in the contract (without accessors) and still take part in
        /// the case-insensitive name clash check. Newtonsoft ignores them completely, so drop them
        /// when another property uses the same name.
        /// </summary>
        private static void RemoveIgnoredNameClashes(JsonTypeInfo typeInfo)
        {
            var ignored = typeInfo.Properties.Where(p => p.Get == null && p.Set == null).ToList();
            foreach (var property in ignored)
            {
                if (typeInfo.Properties.Any(other => other != property && string.Equals(other.Name, property.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    typeInfo.Properties.Remove(property);
                }
            }
        }

        private static JsonConverter? TypeLevelConverter(Type type)
        {
            var attribute = type.GetCustomAttribute<JsonConverterAttribute>(false);
            if (attribute?.ConverterType == null)
            {
                return null;
            }
            return (JsonConverter)Activator.CreateInstance(attribute.ConverterType)!;
        }

        private static bool CoercesEmptyString(Type type)
        {
            if (type.IsValueType || type == typeof(string) || type == typeof(object) || type == typeof(Uri))
            {
                return false;
            }
            // Collections handle the empty string themselves (CompatCollectionConverterFactory).
            return !new CompatCollectionConverterFactory().CanConvert(type);
        }

        internal enum Polymorphism { None, Value, Sequence, StringDictionary }

        private static bool IsPolymorphic(Type type)
        {
            if (type.IsValueType || type == typeof(string) || type == typeof(object) || type.IsSealed || type.IsArray)
            {
                return false;
            }
            if (typeof(IEnumerable).IsAssignableFrom(type))
            {
                return false;
            }
            return type.IsClass || type.IsInterface;
        }

        private static Polymorphism GetPolymorphism(Type type)
        {
            if (IsPolymorphic(type))
            {
                return Polymorphism.Value;
            }

            if (type == typeof(string) || !typeof(IEnumerable).IsAssignableFrom(type) || new CompatCollectionConverterFactory().CanConvert(type))
            {
                // CompatCollectionConverterFactory writes elements by runtime type itself.
                return Polymorphism.None;
            }

            var dictionary = FindGeneric(type, typeof(IDictionary<,>));
            if (dictionary != null)
            {
                var args = dictionary.GetGenericArguments();
                return args[0] == typeof(string) && IsPolymorphic(args[1]) ? Polymorphism.StringDictionary : Polymorphism.None;
            }

            var enumerable = type.IsArray ? null : FindGeneric(type, typeof(IEnumerable<>));
            var element = type.IsArray ? type.GetElementType() : enumerable?.GetGenericArguments()[0];
            return element != null && IsPolymorphic(element) ? Polymorphism.Sequence : Polymorphism.None;
        }

        private static Type? FindGeneric(Type type, Type definition)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == definition)
            {
                return type;
            }
            return type.GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == definition);
        }
    }

    /// <summary>Hands "this JSON null must not be assigned" from a property converter to the property setter.</summary>
    internal static class NullSkip
    {
        [ThreadStatic]
        private static bool pending;

        public static void Mark() => pending = true;

        public static bool Consume()
        {
            bool value = pending;
            pending = false;
            return value;
        }
    }

    internal sealed class CompatPropertyConverter<T> : JsonConverter<T>
    {
        private readonly JsonConverter<T>? explicitConverter;
        private readonly bool skipNull;
        private readonly bool coerceEmptyString;
        private readonly CompatTypeInfoModifier.Polymorphism polymorphism;

        public CompatPropertyConverter(JsonConverter? explicitConverter, bool skipNull, bool coerceEmptyString, CompatTypeInfoModifier.Polymorphism polymorphism)
        {
            this.explicitConverter = explicitConverter as JsonConverter<T>;
            if (explicitConverter != null && this.explicitConverter == null)
            {
                // A converter factory: let System.Text.Json resolve it for this type.
                this.explicitConverter = null;
                factory = explicitConverter;
            }
            this.skipNull = skipNull;
            this.coerceEmptyString = coerceEmptyString;
            this.polymorphism = polymorphism;
        }

        private readonly JsonConverter? factory;

        // Evaluated by the base constructor, before the fields above are set.
        public override bool HandleNull => true;

        private JsonConverter<T> Inner(JsonSerializerOptions options)
        {
            if (explicitConverter != null)
            {
                return explicitConverter;
            }
            if (factory is JsonConverterFactory f)
            {
                return (JsonConverter<T>)f.CreateConverter(typeof(T), options)!;
            }
            return (JsonConverter<T>)options.GetConverter(typeof(T));
        }

        public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                if (skipNull)
                {
                    NullSkip.Mark();
                    return default;
                }
                if (typeof(T).IsValueType)
                {
                    return Inner(options).Read(ref reader, typeToConvert, options);
                }
                return default;
            }

            if (coerceEmptyString && reader.TokenType == JsonTokenType.String && reader.ValueTextEquals(ReadOnlySpan<byte>.Empty))
            {
                return default;
            }

            return Inner(options).Read(ref reader, typeToConvert, options);
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue();
                return;
            }

            switch (polymorphism)
            {
                case CompatTypeInfoModifier.Polymorphism.Value when value.GetType() != typeof(T):
                    JsonSerializer.Serialize(writer, value, value.GetType(), options);
                    return;
                case CompatTypeInfoModifier.Polymorphism.Sequence:
                    writer.WriteStartArray();
                    foreach (var item in (IEnumerable)value)
                    {
                        WriteRuntime(writer, item, options);
                    }
                    writer.WriteEndArray();
                    return;
                case CompatTypeInfoModifier.Polymorphism.StringDictionary:
                    writer.WriteStartObject();
                    foreach (DictionaryEntry entry in (IDictionary)value)
                    {
                        writer.WritePropertyName((string)entry.Key);
                        WriteRuntime(writer, entry.Value, options);
                    }
                    writer.WriteEndObject();
                    return;
            }

            Inner(options).Write(writer, value, options);
        }

        private static void WriteRuntime(Utf8JsonWriter writer, object? item, JsonSerializerOptions options)
        {
            if (item == null)
            {
                writer.WriteNullValue();
            }
            else
            {
                JsonSerializer.Serialize(writer, item, item.GetType(), options);
            }
        }
    }
}

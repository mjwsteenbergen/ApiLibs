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

                bool isCollection = new CompatCollectionConverterFactory().CanConvert(property.PropertyType) && !property.PropertyType.IsArray;

                if (property.Set == null && property.Get != null && !property.PropertyType.IsValueType
                    && member is PropertyInfo { CanWrite: false } or PropertyInfo { SetMethod.IsPublic: false } && !member.IsDefined(typeof(JsonPropertyNameAttribute), true))
                {
                    // Newtonsoft fills an existing object behind a read-only property and skips
                    // anything else (null, or no existing object). A setter that ignores its value
                    // gives System.Text.Json the same behaviour in Populate mode. Collections use
                    // a custom converter, which cannot populate, so their items are merged here.
                    var get = property.Get;
                    property.Set = isCollection ? (obj, value) => MergeInto(get(obj), value) : (_, _) => { };
                    if (!isCollection)
                    {
                        continue;
                    }
                }
                else if (isCollection && property.Get != null && property.Set != null)
                {
                    // Newtonsoft adds to a collection the object already holds instead of replacing it.
                    var get = property.Get;
                    var set = property.Set;
                    property.Set = (obj, value) =>
                    {
                        if (value == null || !MergeInto(get(obj), value))
                        {
                            set(obj, value);
                        }
                    };
                }

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

                bool castFromPrimitive = explicitConverter == null && CastOperators.Has(property.PropertyType);

                if (!skipNull && !coerceEmptyString && polymorphism == Polymorphism.None && !castFromPrimitive)
                {
                    continue;
                }


                var converterType = typeof(CompatPropertyConverter<>).MakeGenericType(property.PropertyType);
                property.CustomConverter = (JsonConverter)Activator.CreateInstance(converterType, explicitConverter, skipNull, coerceEmptyString, polymorphism, castFromPrimitive)!;

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

        /// <summary>Adds the items of a freshly read collection to an existing one. False when there is none to add to.</summary>
        private static bool MergeInto(object? existing, object? value)
        {
            if (existing == null || value == null)
            {
                return false;
            }

            switch (existing)
            {
                case IDictionary target when !target.IsReadOnly && !target.IsFixedSize && value is IDictionary source:
                    foreach (DictionaryEntry entry in source)
                    {
                        target[entry.Key] = entry.Value;
                    }
                    return true;
                case IList target when !target.IsReadOnly && !target.IsFixedSize && value is IEnumerable source:
                    foreach (var item in source)
                    {
                        target.Add(item);
                    }
                    return true;
                default:
                    return false;
            }
        }

        private static bool TypeLevelConverterExists(Type type) => type.GetCustomAttribute<JsonConverterAttribute>(false) != null
            || type == typeof(DateTime) || type == typeof(DateTimeOffset) || type == typeof(decimal) || type == typeof(Guid) || type == typeof(TimeSpan);

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
            if ((type.IsValueType && Nullable.GetUnderlyingType(type) == null) || type == typeof(string) || type == typeof(object) || type == typeof(Uri))
            {
                return false;
            }
            if (Nullable.GetUnderlyingType(type) is Type underlying && (underlying.IsPrimitive || underlying.IsEnum || TypeLevelConverterExists(underlying)))
            {
                // Primitive and enum converters handle empty strings themselves.
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

        private readonly bool castFromPrimitive;

        public CompatPropertyConverter(JsonConverter? explicitConverter, bool skipNull, bool coerceEmptyString, CompatTypeInfoModifier.Polymorphism polymorphism, bool castFromPrimitive)
        {
            this.castFromPrimitive = castFromPrimitive;
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

            if (castFromPrimitive && CastOperators.IsPrimitiveToken(reader.TokenType))
            {
                return (T?)CastOperators.Read(ref reader, typeof(T));
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

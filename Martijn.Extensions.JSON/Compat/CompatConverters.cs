using System;
using System.Buffers;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Martijn.Extensions.JSON.Compat
{
    internal delegate object? TokenRead(ref Utf8JsonReader reader, bool nullable);

    /// <summary>
    /// Converters for primitive types that read and write values the way Newtonsoft.Json 12 does.
    /// Newtonsoft reads most primitives through a JsonReader.ReadAsX method (lenient, e.g. numbers
    /// from strings) and everything else through JsonReader.Read followed by Convert.ChangeType.
    /// </summary>
    internal static class CompatConverters
    {
        public static IEnumerable<JsonConverter> All()
        {
            var list = new List<JsonConverter>
            {
                new CompatStringConverter(),
                new CompatObjectConverter(),
                new CompatUriConverter(),
                new CompatEnumConverterFactory(),
                new CompatCollectionConverterFactory(),
            };

            Struct<int>(list, Tokens.ReadAsInt32, (w, v) => w.WriteNumberValue(v));
            Struct<long>(list, Tokens.ViaRead<long>(), (w, v) => w.WriteNumberValue(v));
            Struct<short>(list, Tokens.ViaInt32<short>(), (w, v) => w.WriteNumberValue(v));
            Struct<byte>(list, Tokens.ViaInt32<byte>(), (w, v) => w.WriteNumberValue(v));
            Struct<double>(list, Tokens.ReadAsDouble, Tokens.WriteDouble);
            Struct<float>(list, Tokens.ViaDouble<float>(), Tokens.WriteFloat);
            Struct<decimal>(list, Tokens.ReadAsDecimal, Tokens.WriteDecimal);
            Struct<bool>(list, Tokens.ReadAsBoolean, (w, v) => w.WriteBooleanValue(v));
            Struct<DateTime>(list, Tokens.ReadAsDateTime, (w, v) => w.WriteStringValue(NewtonsoftDates.Format(v)));
            Struct<DateTimeOffset>(list, Tokens.ReadAsDateTimeOffset, (w, v) => w.WriteStringValue(NewtonsoftDates.Format(v)));
            Struct<uint>(list, Tokens.ViaRead<uint>(), (w, v) => w.WriteNumberValue(v));
            Struct<ulong>(list, Tokens.ViaRead<ulong>(), (w, v) => w.WriteNumberValue(v));
            Struct<ushort>(list, Tokens.ViaRead<ushort>(), (w, v) => w.WriteNumberValue(v));
            Struct<sbyte>(list, Tokens.ViaRead<sbyte>(), (w, v) => w.WriteNumberValue(v));
            Struct<char>(list, Tokens.ViaRead<char>(), (w, v) => w.WriteStringValue(v.ToString()));
            Struct<Guid>(list, Tokens.ViaRead<Guid>(), (w, v) => w.WriteStringValue(v.ToString("D")));
            Struct<TimeSpan>(list, Tokens.ViaRead<TimeSpan>(), (w, v) => w.WriteStringValue(v.ToString(null, CultureInfo.InvariantCulture)));
            return list;
        }

        private static void Struct<T>(List<JsonConverter> list, TokenRead read, Action<Utf8JsonWriter, T> write) where T : struct
        {
            list.Add(new CompatStructConverter<T>(read, write));
            list.Add(new CompatNullableStructConverter<T>(read, write));
        }
    }

    internal sealed class CompatStructConverter<T> : JsonConverter<T> where T : struct
    {
        private readonly TokenRead read;
        private readonly Action<Utf8JsonWriter, T> write;

        public CompatStructConverter(TokenRead read, Action<Utf8JsonWriter, T> write)
        {
            this.read = read;
            this.write = write;
        }

        public override bool HandleNull => true;

        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            object? value = read(ref reader, false);
            if (value == null)
            {
                throw new JsonException($"Error converting value {{null}} to type '{typeof(T)}'.");
            }
            return (T)value;
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options) => write(writer, value);

        public override T ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var key = reader.GetString()!;
            var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(key));
            var inner = new Utf8JsonReader(bytes);
            inner.Read();
            return Read(ref inner, typeToConvert, options);
        }

        public override void WriteAsPropertyName(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            writer.WritePropertyName(Convert.ToString(value, CultureInfo.InvariantCulture) ?? "");
        }
    }

    internal sealed class CompatNullableStructConverter<T> : JsonConverter<T?> where T : struct
    {
        private readonly TokenRead read;
        private readonly Action<Utf8JsonWriter, T> write;

        public CompatNullableStructConverter(TokenRead read, Action<Utf8JsonWriter, T> write)
        {
            this.read = read;
            this.write = write;
        }

        public override bool HandleNull => true;

        public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            object? value = read(ref reader, true);
            return value == null ? null : (T)value;
        }

        public override void Write(Utf8JsonWriter writer, T? value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue();
            }
            else
            {
                write(writer, value.Value);
            }
        }
    }

    internal sealed class CompatStringConverter : JsonConverter<string>
    {
        public override bool HandleNull => true;

        public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            => (string?)Tokens.ReadAsString(ref reader, true);

        public override void Write(Utf8JsonWriter writer, string? value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue();
            }
            else
            {
                writer.WriteStringValue(value);
            }
        }

        public override string ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => reader.GetString()!;

        public override void WriteAsPropertyName(Utf8JsonWriter writer, string value, JsonSerializerOptions options) => writer.WritePropertyName(value);
    }

    internal sealed class CompatUriConverter : JsonConverter<Uri>
    {
        private static readonly TokenRead ReadUri = Tokens.ViaRead<Uri>();

        public override bool HandleNull => true;

        public override Uri? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => (Uri?)ReadUri(ref reader, true);

        public override void Write(Utf8JsonWriter writer, Uri? value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue();
            }
            else
            {
                writer.WriteStringValue(value.OriginalString);
            }
        }
    }

    /// <summary>
    /// Values of properties typed <c>object</c>. Newtonsoft gives primitives (long, double, string,
    /// bool, DateTime for ISO date strings) and JObject/JArray for objects and arrays. JObject cannot
    /// exist here, so objects and arrays become a JsonElement (see QUIRKS.md).
    /// </summary>
    internal sealed class CompatObjectConverter : JsonConverter<object>
    {
        public override bool HandleNull => true;

        public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.StartObject:
                case JsonTokenType.StartArray:
                    using (var doc = JsonDocument.ParseValue(ref reader))
                    {
                        return doc.RootElement.Clone();
                    }
                default:
                    return Tokens.ReadValue(ref reader);
            }
        }

        public override void Write(Utf8JsonWriter writer, object? value, JsonSerializerOptions options)
        {
            switch (value)
            {
                case null:
                    writer.WriteNullValue();
                    break;
                case JsonElement element:
                    element.WriteTo(writer);
                    break;
                default:
                    if (value.GetType() == typeof(object))
                    {
                        writer.WriteStartObject();
                        writer.WriteEndObject();
                    }
                    else
                    {
                        JsonSerializer.Serialize(writer, value, value.GetType(), options);
                    }
                    break;
            }
        }
    }

    internal sealed class CompatEnumConverterFactory : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            var t = Nullable.GetUnderlyingType(typeToConvert) ?? typeToConvert;
            return t.IsEnum;
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var underlying = Nullable.GetUnderlyingType(typeToConvert);
            var type = underlying == null
                ? typeof(EnumConverter<>).MakeGenericType(typeToConvert)
                : typeof(NullableEnumConverter<>).MakeGenericType(underlying);
            return (JsonConverter)Activator.CreateInstance(type)!;
        }

        private static void WriteEnum(Utf8JsonWriter writer, Enum value)
        {
            var code = Convert.GetTypeCode(value);
            if (code == TypeCode.UInt64)
            {
                writer.WriteNumberValue(Convert.ToUInt64(value, CultureInfo.InvariantCulture));
            }
            else
            {
                writer.WriteNumberValue(Convert.ToInt64(value, CultureInfo.InvariantCulture));
            }
        }

        private sealed class EnumConverter<T> : JsonConverter<T> where T : struct, Enum
        {
            public override bool HandleNull => true;

            public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                var v = Tokens.ReadEnum(ref reader, typeof(T), false);
                return v == null ? throw new JsonException($"Error converting value {{null}} to type '{typeof(T)}'.") : (T)v;
            }

            public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options) => WriteEnum(writer, value);

            public override T ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
                => (T)Tokens.ParseEnum(typeof(T), reader.GetString()!);

            public override void WriteAsPropertyName(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
                => writer.WritePropertyName(value.ToString());
        }

        private sealed class NullableEnumConverter<T> : JsonConverter<T?> where T : struct, Enum
        {
            public override bool HandleNull => true;

            public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                var v = Tokens.ReadEnum(ref reader, typeof(T), true);
                return v == null ? null : (T)v;
            }

            public override void Write(Utf8JsonWriter writer, T? value, JsonSerializerOptions options)
            {
                if (value == null)
                {
                    writer.WriteNullValue();
                }
                else
                {
                    WriteEnum(writer, value.Value);
                }
            }
        }
    }

    /// <summary>Token-level reading rules of Newtonsoft's JsonTextReader and JsonSerializerInternalReader.</summary>
    internal static class Tokens
    {
        private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

        public static string RawText(ref Utf8JsonReader reader)
        {
            return reader.HasValueSequence
                ? Encoding.UTF8.GetString(reader.ValueSequence.ToArray())
                : Encoding.UTF8.GetString(reader.ValueSpan.ToArray());
        }

        private static bool IsIntegerText(string raw) => raw.IndexOfAny(new[] { '.', 'e', 'E' }) == -1;

        private static JsonException Unexpected(ref Utf8JsonReader reader, string what)
            => new JsonException($"Unexpected token {reader.TokenType} when reading {what}.");

        /// <summary>The value JsonTextReader.Read() produces for a primitive token.</summary>
        public static object? ReadValue(ref Utf8JsonReader reader)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.String:
                    var s = reader.GetString()!;
                    return NewtonsoftDates.TryParseDateTime(s, out var dt) ? dt : s;
                case JsonTokenType.Number:
                    var raw = RawText(ref reader);
                    if (IsIntegerText(raw))
                    {
                        if (long.TryParse(raw, NumberStyles.AllowLeadingSign, Culture, out long l))
                        {
                            return l;
                        }
                        return BigInteger.Parse(raw, Culture);
                    }
                    return double.Parse(raw, NumberStyles.Float, Culture);
                case JsonTokenType.True:
                    return true;
                case JsonTokenType.False:
                    return false;
                case JsonTokenType.Null:
                    return null;
                default:
                    throw Unexpected(ref reader, "a value");
            }
        }

        public static object? ReadAsString(ref Utf8JsonReader reader, bool nullable)
        {
            return reader.TokenType switch
            {
                JsonTokenType.String => reader.GetString(),
                JsonTokenType.Number => RawText(ref reader),
                JsonTokenType.True => "true",
                JsonTokenType.False => "false",
                JsonTokenType.Null => null,
                _ => throw Unexpected(ref reader, "a string"),
            };
        }

        public static object? ReadAsInt32(ref Utf8JsonReader reader, bool nullable)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.Null:
                    return null;
                case JsonTokenType.Number:
                    var raw = RawText(ref reader);
                    if (IsIntegerText(raw) && int.TryParse(raw, NumberStyles.AllowLeadingSign, Culture, out int i))
                    {
                        return i;
                    }
                    throw new JsonException($"Input string '{raw}' is not a valid integer.");
                case JsonTokenType.String:
                    var s = reader.GetString();
                    if (string.IsNullOrEmpty(s))
                    {
                        return null;
                    }
                    if (int.TryParse(s, NumberStyles.Integer, Culture, out int fromString))
                    {
                        return fromString;
                    }
                    throw new JsonException($"Could not convert string to integer: {s}.");
                default:
                    throw Unexpected(ref reader, "an integer");
            }
        }

        public static object? ReadAsDouble(ref Utf8JsonReader reader, bool nullable)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.Null:
                    return null;
                case JsonTokenType.Number:
                    return double.Parse(RawText(ref reader), NumberStyles.Float, Culture);
                case JsonTokenType.String:
                    var s = reader.GetString();
                    if (string.IsNullOrEmpty(s))
                    {
                        return null;
                    }
                    if (double.TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, Culture, out double d))
                    {
                        return d;
                    }
                    throw new JsonException($"Could not convert string to double: {s}.");
                default:
                    throw Unexpected(ref reader, "a double");
            }
        }

        public static object? ReadAsDecimal(ref Utf8JsonReader reader, bool nullable)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.Null:
                    return null;
                case JsonTokenType.Number:
                    var raw = RawText(ref reader);
                    if (decimal.TryParse(raw, NumberStyles.Number | NumberStyles.AllowExponent, Culture, out decimal d))
                    {
                        return d;
                    }
                    throw new JsonException($"Input string '{raw}' is not a valid decimal.");
                case JsonTokenType.String:
                    var s = reader.GetString();
                    if (string.IsNullOrEmpty(s))
                    {
                        return null;
                    }
                    if (decimal.TryParse(s, NumberStyles.Number, Culture, out decimal fromString)
                        || decimal.TryParse(s, NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent, Culture, out fromString))
                    {
                        return fromString;
                    }
                    throw new JsonException($"Could not convert string to decimal: {s}.");
                default:
                    throw Unexpected(ref reader, "a decimal");
            }
        }

        public static object? ReadAsBoolean(ref Utf8JsonReader reader, bool nullable)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.Null:
                    return null;
                case JsonTokenType.True:
                    return true;
                case JsonTokenType.False:
                    return false;
                case JsonTokenType.Number:
                    var value = ReadValue(ref reader);
                    return value is BigInteger big ? !big.IsZero : Convert.ToBoolean(value, Culture);
                case JsonTokenType.String:
                    var s = reader.GetString();
                    if (string.IsNullOrEmpty(s))
                    {
                        return null;
                    }
                    if (bool.TryParse(s, out bool b))
                    {
                        return b;
                    }
                    throw new JsonException($"Could not convert string to boolean: {s}.");
                default:
                    throw Unexpected(ref reader, "a boolean");
            }
        }

        public static object? ReadAsDateTime(ref Utf8JsonReader reader, bool nullable)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.Null:
                    return null;
                case JsonTokenType.String:
                    var s = reader.GetString();
                    if (string.IsNullOrEmpty(s))
                    {
                        return null;
                    }
                    if (NewtonsoftDates.TryReadDateTimeString(s!, out var dt))
                    {
                        return dt;
                    }
                    throw new JsonException($"Could not convert string to DateTime: {s}.");
                default:
                    throw Unexpected(ref reader, "a DateTime");
            }
        }

        public static object? ReadAsDateTimeOffset(ref Utf8JsonReader reader, bool nullable)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.Null:
                    return null;
                case JsonTokenType.String:
                    var s = reader.GetString();
                    if (string.IsNullOrEmpty(s))
                    {
                        return null;
                    }
                    if (NewtonsoftDates.TryReadDateTimeOffsetString(s!, out var dt))
                    {
                        return dt;
                    }
                    throw new JsonException($"Could not convert string to DateTimeOffset: {s}.");
                default:
                    throw Unexpected(ref reader, "a DateTimeOffset");
            }
        }

        /// <summary>ReadAsInt32 followed by the serializer's conversion (byte, short).</summary>
        public static TokenRead ViaInt32<T>() => (ref Utf8JsonReader reader, bool nullable) =>
        {
            var v = ReadAsInt32(ref reader, nullable);
            return v == null ? null : ChangeType(v, typeof(T));
        };

        /// <summary>ReadAsDouble followed by the serializer's conversion (float).</summary>
        public static TokenRead ViaDouble<T>() => (ref Utf8JsonReader reader, bool nullable) =>
        {
            var v = ReadAsDouble(ref reader, nullable);
            return v == null ? null : ChangeType(v, typeof(T));
        };

        /// <summary>JsonReader.Read() followed by JsonSerializerInternalReader.EnsureType.</summary>
        public static TokenRead ViaRead<T>() => (ref Utf8JsonReader reader, bool nullable) =>
        {
            if (reader.TokenType is JsonTokenType.StartObject or JsonTokenType.StartArray)
            {
                throw Unexpected(ref reader, typeof(T).Name);
            }
            bool isString = reader.TokenType == JsonTokenType.String;
            var value = ReadValue(ref reader);
            if (isString && value is string s && s.Length == 0 && nullable)
            {
                return null;
            }
            return EnsureType(value, typeof(T), nullable);
        };

        public static object? ReadEnum(ref Utf8JsonReader reader, Type enumType, bool nullable)
        {
            if (reader.TokenType is JsonTokenType.StartObject or JsonTokenType.StartArray)
            {
                throw Unexpected(ref reader, enumType.Name);
            }
            bool isString = reader.TokenType == JsonTokenType.String;
            var value = ReadValue(ref reader);
            if (isString && value is string s && s.Length == 0 && nullable)
            {
                return null;
            }
            if (value == null)
            {
                return nullable ? null : throw new JsonException($"Error converting value {{null}} to type '{enumType}'.");
            }

            try
            {
                if (value is string text)
                {
                    return ParseEnum(enumType, text);
                }
                // Newtonsoft calls Enum.ToObject for every value (integers, but also booleans and chars).
                return Enum.ToObject(enumType, value);
            }
            catch (Exception e) when (e is not JsonException)
            {
                throw new JsonException($"Error converting value {value} to type '{enumType}'.", e);
            }
        }

        public static object? EnsureType(object? value, Type targetType, bool nullable)
        {
            if (value == null)
            {
                if (nullable)
                {
                    return null;
                }
                throw new JsonException($"Error converting value {{null}} to type '{targetType}'.");
            }

            if (value.GetType() == targetType)
            {
                return value;
            }

            try
            {
                if (typeof(IConvertible).IsAssignableFrom(targetType))
                {
                    return ChangeType(value, targetType);
                }

                if (value is string s)
                {
                    if (targetType == typeof(Guid))
                    {
                        return new Guid(s);
                    }
                    if (targetType == typeof(Uri))
                    {
                        return new Uri(s, UriKind.RelativeOrAbsolute);
                    }
                    if (targetType == typeof(TimeSpan))
                    {
                        return TimeSpan.Parse(s, Culture);
                    }
                }
            }
            catch (Exception e) when (e is not JsonException)
            {
                throw new JsonException($"Error converting value {value} to type '{targetType}'.", e);
            }

            throw new JsonException($"Error converting value {value} to type '{targetType}'.");
        }

        private static object ChangeType(object value, Type targetType)
        {
            try
            {
                if (value is BigInteger big)
                {
                    return FromBigInteger(big, targetType);
                }
                return Convert.ChangeType(value, targetType, Culture);
            }
            catch (Exception e)
            {
                throw new JsonException($"Error converting value {value} to type '{targetType}'.", e);
            }
        }

        private static object FromBigInteger(BigInteger i, Type targetType)
        {
            if (targetType == typeof(decimal))
            {
                return (decimal)i;
            }
            if (targetType == typeof(double))
            {
                return (double)i;
            }
            if (targetType == typeof(float))
            {
                return (float)i;
            }
            if (targetType == typeof(ulong))
            {
                return (ulong)i;
            }
            if (targetType == typeof(bool))
            {
                return i != 0;
            }
            return Convert.ChangeType((long)i, targetType, Culture);
        }

        /// <summary>Port of Newtonsoft's EnumUtils.ParseEnum without naming strategies or EnumMember.</summary>
        public static object ParseEnum(Type enumType, string value)
        {
            string[] names = Enum.GetNames(enumType);
            ulong[] values = names.Select(n => ToUInt64(Enum.Parse(enumType, n))).ToArray();

            int? matching = FindIndexByName(names, value, 0, value.Length, StringComparison.Ordinal);
            if (matching != null)
            {
                return Enum.ToObject(enumType, values[matching.Value]);
            }

            int first = -1;
            for (int i = 0; i < value.Length; i++)
            {
                if (!char.IsWhiteSpace(value[i]))
                {
                    first = i;
                    break;
                }
            }
            if (first == -1)
            {
                throw new ArgumentException("Must specify valid information for parsing in the string.");
            }

            char firstChar = value[first];
            if (char.IsDigit(firstChar) || firstChar == '-' || firstChar == '+')
            {
                Type underlying = Enum.GetUnderlyingType(enumType);
                value = value.Trim();
                object? temp = null;
                try
                {
                    temp = Convert.ChangeType(value, underlying, Culture);
                }
                catch (FormatException)
                {
                }
                if (temp != null)
                {
                    return Enum.ToObject(enumType, temp);
                }
            }

            ulong result = 0;
            int valueIndex = first;
            while (valueIndex <= value.Length)
            {
                int endIndex = value.IndexOf(',', valueIndex);
                if (endIndex == -1)
                {
                    endIndex = value.Length;
                }
                int endNoWhitespace = endIndex;
                while (valueIndex < endIndex && char.IsWhiteSpace(value[valueIndex]))
                {
                    valueIndex++;
                }
                while (endNoWhitespace > valueIndex && char.IsWhiteSpace(value[endNoWhitespace - 1]))
                {
                    endNoWhitespace--;
                }
                int length = endNoWhitespace - valueIndex;

                matching = FindIndexByName(names, value, valueIndex, length, StringComparison.Ordinal)
                           ?? FindIndexByName(names, value, valueIndex, length, StringComparison.OrdinalIgnoreCase);
                if (matching == null)
                {
                    matching = FindIndexByName(names, value, 0, value.Length, StringComparison.OrdinalIgnoreCase);
                    if (matching != null)
                    {
                        return Enum.ToObject(enumType, values[matching.Value]);
                    }
                    throw new ArgumentException($"Requested value '{value}' was not found.");
                }

                result |= values[matching.Value];
                valueIndex = endIndex + 1;
            }

            return Enum.ToObject(enumType, result);
        }

        private static ulong ToUInt64(object value)
        {
            return Convert.GetTypeCode(value) switch
            {
                TypeCode.SByte => (ulong)(sbyte)value,
                TypeCode.Byte => (byte)value,
                TypeCode.Boolean => Convert.ToByte((bool)value),
                TypeCode.Int16 => (ulong)(short)value,
                TypeCode.UInt16 => (ushort)value,
                TypeCode.Char => (char)value,
                TypeCode.UInt32 => (uint)value,
                TypeCode.Int32 => (ulong)(int)value,
                TypeCode.UInt64 => (ulong)value,
                TypeCode.Int64 => (ulong)(long)value,
                _ => throw new InvalidOperationException("Unknown enum type."),
            };
        }

        private static int? FindIndexByName(string[] names, string value, int start, int length, StringComparison comparison)
        {
            for (int i = 0; i < names.Length; i++)
            {
                if (names[i].Length == length && string.Compare(names[i], 0, value, start, length, comparison) == 0)
                {
                    return i;
                }
            }
            return null;
        }

        public static void WriteDouble(Utf8JsonWriter writer, double value)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                writer.WriteStringValue(value.ToString("R", Culture));
                return;
            }
            writer.WriteRawValue(EnsureDecimalPlace(value.ToString("R", Culture)), skipInputValidation: true);
        }

        public static void WriteFloat(Utf8JsonWriter writer, float value)
        {
            if (float.IsNaN(value) || float.IsInfinity(value))
            {
                writer.WriteStringValue(value.ToString("R", Culture));
                return;
            }
            writer.WriteRawValue(EnsureDecimalPlace(value.ToString("R", Culture)), skipInputValidation: true);
        }

        public static void WriteDecimal(Utf8JsonWriter writer, decimal value)
        {
            string text = value.ToString(Culture);
            writer.WriteRawValue(text.IndexOf('.') != -1 ? text : text + ".0", skipInputValidation: true);
        }

        private static string EnsureDecimalPlace(string text)
            => text.IndexOf('.') != -1 || text.IndexOf('E') != -1 || text.IndexOf('e') != -1 ? text : text + ".0";
    }
}

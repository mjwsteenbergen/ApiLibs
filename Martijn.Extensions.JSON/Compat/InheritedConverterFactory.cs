using System;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Martijn.Extensions.JSON.Compat
{
    /// <summary>
    /// Newtonsoft applies a [JsonConverter] declared on a base class to every subclass;
    /// System.Text.Json only looks at the type itself. For a subclass without its own attribute
    /// this hands the work to the base class's converter.
    /// </summary>
    internal sealed class InheritedConverterFactory : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return !typeToConvert.IsValueType
                   && typeToConvert.GetCustomAttribute<JsonConverterAttribute>(false) == null
                   && Inherited(typeToConvert) != null;
        }

        private static JsonConverterAttribute? Inherited(Type type)
        {
            for (var t = type.BaseType; t != null && t != typeof(object); t = t.BaseType)
            {
                var attribute = t.GetCustomAttribute<JsonConverterAttribute>(false);
                if (attribute?.ConverterType != null)
                {
                    return attribute;
                }
            }
            return null;
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var inner = (JsonConverter)Activator.CreateInstance(Inherited(typeToConvert)!.ConverterType!)!;
            if (inner is JsonConverterFactory factory)
            {
                inner = factory.CreateConverter(typeToConvert, options)!;
            }

            var baseType = inner.GetType();
            while (baseType != null && !(baseType.IsGenericType && baseType.GetGenericTypeDefinition() == typeof(JsonConverter<>)))
            {
                baseType = baseType.BaseType;
            }
            if (baseType == null)
            {
                return null;
            }

            var handled = baseType.GetGenericArguments()[0];
            return (JsonConverter)Activator.CreateInstance(typeof(Adapter<,>).MakeGenericType(typeToConvert, handled), inner)!;
        }

        private sealed class Adapter<TDerived, TBase> : JsonConverter<TDerived> where TDerived : TBase
        {
            private readonly JsonConverter<TBase> inner;

            public Adapter(JsonConverter<TBase> inner)
            {
                this.inner = inner;
            }

            // Read by the base constructor before 'inner' is set; nulls are routed by hand below.
            public override bool HandleNull => true;

            // Newtonsoft returned whatever the converter produced; a different subclass fails the cast.
            public override TDerived? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Null && !inner.HandleNull)
                {
                    return default;
                }
                return (TDerived?)inner.Read(ref reader, typeof(TBase), options);
            }

            public override void Write(Utf8JsonWriter writer, TDerived value, JsonSerializerOptions options)
            {
                if (value == null && !inner.HandleNull)
                {
                    writer.WriteNullValue();
                    return;
                }
                inner.Write(writer, value!, options);
            }
        }
    }
}

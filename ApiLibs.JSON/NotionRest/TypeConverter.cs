using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Martijn.Extensions.JSON.Compat;

namespace ApiLibs.JSON.NotionRest;

public abstract class NotionUnionTypeWrapper<T> where T : WithType
{
    public T Value { get; set; }
}

public interface WithType
{
    [JsonIgnore]
    public string Type { get; set; }
}

/// <summary>
/// Converts { type: "blockName", "blockName: { ...someContent } }
/// to
/// { value: { ...someContent, type: "blockName" } }
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class NotionObjectWithTypeConverter<T, Y> : JsonConverter<Y> where T : WithType where Y : NotionUnionTypeWrapper<T>,new()
{
    public abstract T Read(string type, JsonElement token);

    public override Y Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        JsonElement jObject;
        using (var document = JsonDocument.ParseValue(ref reader))
        {
            jObject = document.RootElement.Clone();
        }

        if (jObject.ValueKind is JsonValueKind.Null)
        {
            return null;
        }

        string type = null;

        try
        {
            if (jObject.ValueKind is not JsonValueKind.Undefined and not JsonValueKind.Null)
            {
                // Was jObject["type"].ToObject<string>(); a missing "type" or a non-object threw and was caught.
                type = NewtonsoftPopulate.ValueAsString(jObject.GetProperty("type"));
            }
        }
        catch { }

        T result = Read(type, jObject);

        NewtonsoftPopulate.Populate(jObject, result);
        // Was jObject[type]: a null type threw ArgumentNullException, a missing member a NullReferenceException.
        if (type == null)
        {
            throw new ArgumentNullException("propertyName");
        }
        if (!jObject.TryGetProperty(type, out var inner))
        {
            throw new NullReferenceException();
        }
        NewtonsoftPopulate.Populate(inner, result);
        return new Y
        {
            Value = result
        };
    }

    public override void Write(Utf8JsonWriter writer, Y value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        writer.WritePropertyName("type");
        if (value.Value.Type == null)
        {
            writer.WriteNullValue();
        }
        else
        {
            writer.WriteStringValue(value.Value.Type);
        }

        writer.WritePropertyName(value.Value.Type);
        writer.WriteRawValue(NewtonsoftCompat.Serialize(value.Value, ignoreNullValues: true), skipInputValidation: true);
        writer.WriteEndObject();
    }
}

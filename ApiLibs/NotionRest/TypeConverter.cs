using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ApiLibs.NotionRest;

public abstract class NotionUnionTypeWrapper<T> where T : WithType
{
    public T Value { get; set; }
}

public interface WithType
{
    [JsonIgnore()]
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
    public abstract T Read(string type, JToken token);

    public override Y ReadJson(JsonReader reader, Type objectType, Y existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        JToken jObject = JToken.ReadFrom(reader);

        if (jObject.Type is JTokenType.Null)
        {
            return null; 
        }

        string type = null;

        try
        {
            if (jObject.Type is not JTokenType.None and not JTokenType.Null)
            {
                type = jObject["type"].ToObject<string>();
            }
        }
        catch { }

        T result = Read(type, jObject);


        serializer.Populate(jObject.CreateReader(), result);
        serializer.Populate(jObject[type].CreateReader(), result);
        return new Y
        {
            Value = result
        };
    }

    public override bool CanWrite => true;

    public override void WriteJson(JsonWriter writer, Y value, JsonSerializer serializer)
    {
        writer.WriteStartObject();

        writer.WritePropertyName("type");
        writer.WriteValue(value.Value.Type);
        

        writer.WritePropertyName(value.Value.Type);
        writer.WriteRawValue(JsonConvert.SerializeObject(value.Value, Formatting.None, new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        }));
        writer.WriteEndObject();
    }
}
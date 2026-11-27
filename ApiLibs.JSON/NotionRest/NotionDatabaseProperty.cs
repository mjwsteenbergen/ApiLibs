using System;
using System.Text.Json.Serialization;
using System.Text.Json;
using Martijn.Extensions.JSON.Compat;

namespace ApiLibs.JSON.NotionRest
{
    [JsonConverter(typeof(NotionDatabasePropertyConverter))]
    public abstract class NotionDatabaseProperty
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class TitleDatabaseProperty : NotionDatabaseProperty {

    }
    
    public class EmailDatabaseProperty : NotionDatabaseProperty {

    }

    public class PhoneDatabaseProperty : NotionDatabaseProperty {

    }

    public class RichTextDatabaseProperty : NotionDatabaseProperty
    {

    }

    public class CheckboxDatabaseProperty : NotionDatabaseProperty
    {

    }

    public class NumberDatabaseProperty : NotionDatabaseProperty
    {
        [JsonPropertyName("number")]
        public NumberDatabasePropertyValue Number { get; set;}
    }

    public partial class NumberDatabasePropertyValue
    {
        [JsonPropertyName("format")]
        public string Format { get; set; }
    }

    public class SelectDatabaseProperty : NotionDatabaseProperty
    {
        [JsonPropertyName("select")]
        public SelectDatabasePropertyValue Select { get; set;}
    }

    public class SelectDatabasePropertyValue
    {
        [JsonPropertyName("options")]
        public Option[] Options { get; set; }
    }

    public class MultiSelectDatabaseProperty : NotionDatabaseProperty
    {
        [JsonPropertyName("multi_select")]
        public MultiSelectDatabasePropertyValue MultiSelect { get; set;}
    }

    public class LastEditedTimeDatabaseProperty : NotionDatabaseProperty
    {
    }
    
    public class FilesDatabaseProperty : NotionDatabaseProperty
    {
    }

    public class MultiSelectDatabasePropertyValue
    {
        [JsonPropertyName("options")]
        public Option[] Options { get; set; }
    }

    public class RelationDatabaseProperty : NotionDatabaseProperty
    {
        [JsonPropertyName("relation")]
        public RelationDatabasePropertyValue Relation { get; set;}
    }

    public class RelationDatabasePropertyValue
    {
        [JsonPropertyName("database_id")]
        public string DatabaseId { get; set;}
    }

    public class UrlDatabaseProperty : NotionDatabaseProperty
    {

    }

    public class DateDatabaseProperty : NotionDatabaseProperty
    {

    }

    public class NotionDatabasePropertyConverter : JsonConverter<NotionDatabaseProperty>
    {
        class NotionPropertyImp : NotionProperty { }

        // Newtonsoft passed JSON null to the converter too (which then threw: there is no "null" type).
        public override bool HandleNull => true;

        public override NotionDatabaseProperty Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            JsonElement jObject;
            using (var document = JsonDocument.ParseValue(ref reader))
            {
                jObject = document.RootElement.Clone();
            }

            string type = null;

            try
            {
                if (jObject.ValueKind is not JsonValueKind.Undefined and not JsonValueKind.Null)
                {
                    type = NewtonsoftPopulate.ValueAsString(jObject.GetProperty("type"));
                }
            }
            catch { }

            NotionDatabaseProperty result = type switch
            {
                "checkbox" => new CheckboxDatabaseProperty(),
                "date" => new DateDatabaseProperty(),
                "email" => new EmailDatabaseProperty(),
                "files" => new FilesDatabaseProperty(),
                "last_edited_time" => new LastEditedTimeDatabaseProperty(),
                "multi_select" => new MultiSelectDatabaseProperty(),
                "number" => new NumberDatabaseProperty(),
                "phone_number" => new PhoneDatabaseProperty(),
                "relation" => new RelationDatabaseProperty(),
                "rich_text" => new RichTextDatabaseProperty(),
                "select" => new SelectDatabaseProperty(),
                "title" => new TitleDatabaseProperty(),
                "url" => new UrlDatabaseProperty(),
                _ => throw new ArgumentOutOfRangeException("Cannot convert type " + type + jObject.ToString())
            };

            NewtonsoftPopulate.Populate(jObject, result);

            return result;
        }

        /// <summary>The Newtonsoft version had CanWrite => false: properties are written member by member.</summary>
        public override void Write(Utf8JsonWriter writer, NotionDatabaseProperty value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue();
                return;
            }
            NewtonsoftPopulate.WriteDefault(writer, value, options);
        }
    }
}
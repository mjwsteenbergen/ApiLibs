using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ApiLibs.NotionRest
{
    [JsonConverter(typeof(NotionDatabasePropertyConverter))]
    public abstract class NotionDatabaseProperty
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
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
        [JsonProperty("number")]
        public NumberDatabasePropertyValue Number { get; set;}
    }

    public partial class NumberDatabasePropertyValue
    {
        [JsonProperty("format")]
        public string Format { get; set; }
    }

    public class SelectDatabaseProperty : NotionDatabaseProperty
    {
        [JsonProperty("select")]
        public SelectDatabasePropertyValue Select { get; set;}
    }

    public class SelectDatabasePropertyValue
    {
        [JsonProperty("options")]
        public Option[] Options { get; set; }
    }

    public class MultiSelectDatabaseProperty : NotionDatabaseProperty
    {
        [JsonProperty("multi_select")]
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
        [JsonProperty("options")]
        public Option[] Options { get; set; }
    }

    public class RelationDatabaseProperty : NotionDatabaseProperty
    {
        [JsonProperty("relation")]
        public RelationDatabasePropertyValue Relation { get; set;}
    }

    public class RelationDatabasePropertyValue
    {
        [JsonProperty("database_id")]
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

        public override NotionDatabaseProperty ReadJson(JsonReader reader, Type objectType, NotionDatabaseProperty existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JToken jObject = JToken.ReadFrom(reader);

            string type = null;

            try
            {
                if (jObject.Type is not JTokenType.None and not JTokenType.Null)
                {
                    type = jObject["type"].ToObject<string>();
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

            serializer.Populate(jObject.CreateReader(), result);

            return result;
        }

        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, NotionDatabaseProperty value, JsonSerializer serializer)
        {
            throw new System.NotImplementedException();
        }
    }
}
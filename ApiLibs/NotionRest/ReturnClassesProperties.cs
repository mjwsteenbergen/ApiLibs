using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace ApiLibs.NotionRest
{
    [JsonConverter(typeof(NotionPropertyConverter))]
    public abstract class NotionProperty
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("id")]
        public string Id { get; set;}

        [JsonProperty("name")]
        public string Name { get; set;}
    }

    public class NotionPropertyConverter : JsonConverter<NotionProperty>
    {
        class NotionPropertyImp : NotionProperty { }

        public override NotionProperty ReadJson(JsonReader reader, Type objectType, NotionProperty existingValue, bool hasExistingValue, JsonSerializer serializer)
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

            NotionProperty result = type switch
            {
                "title" =>  new TitleProperty(),
                "rich_text" => new RichTextProperty(),
                "checkbox" => new CheckboxProperty(),
                "select" => new SelectProperty(),
                "relation" => new RelationProperty(),
                "url" => new UrlProperty(),
                "date" => new DateProperty(),
                _ => throw new ArgumentOutOfRangeException("Cannot convert type " + type + jObject.ToString())
            };

            serializer.Populate(jObject.CreateReader(), result);

            if(result is DateProperty dprop) {
                result = new DatePropertyConverter().ReadJson(jObject["date"].CreateReader(), typeof(DateProperty), dprop, false, serializer);
            }


            return result;
        }

        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, NotionProperty value, JsonSerializer serializer)
        {
            throw new System.NotImplementedException();
        }
    }

    public class TitleProperty : NotionProperty
    {

        [JsonProperty("title")]
        public List<RichText> Title { get; set; }
    }

    public class UrlProperty : NotionProperty {

    }

    [JsonConverter(typeof(DatePropertyConverter))]
    public class DateProperty : NotionProperty
    {
        [JsonProperty("start")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? Start { get; set;}

        [JsonProperty("end")]
        public DateTime? End { get; set;}
    }

    public class DateTimeConverter : JsonConverter<DateTime?>
    {
        public override DateTime? ReadJson(JsonReader reader, Type objectType, DateTime? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JToken jObject = JToken.ReadFrom(reader);
            string val = jObject.Value<string>();
            return DatePropertyConverter.GetDateTime(val);
        }

        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, DateTime? value, JsonSerializer serializer)
        {
            throw new System.NotImplementedException();
        }
    }

    public class DatePropertyConverter : JsonConverter<DateProperty>
    {
        public override DateProperty ReadJson(JsonReader reader, Type objectType, DateProperty existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JToken jObject = JToken.ReadFrom(reader);

            serializer.Populate(jObject.CreateReader(), existingValue);

            return existingValue;
        }

        public static DateTime? GetDateTime(string date) {
            if (date == null)
            {
                return null;
            }
            var match = Regex.Match(date, @"(\d{4})-(\d{2})-(\d{2})");
            int year = int.Parse(match.Groups[1].Value);
            int month = int.Parse(match.Groups[2].Value);
            int day = int.Parse(match.Groups[3].Value);
            return new DateTime(year, month, day);
        }

        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, DateProperty value, JsonSerializer serializer)
        {
            throw new System.NotImplementedException();
        }
    }

    public partial class RichTextProperty : NotionProperty
    {

        [JsonProperty("rich_text")]
        public List<RichText> RichText { get; set; }
    }

    public class RichText
    {
        [JsonProperty("plain_text")]
        public string PlainText { get; set;}

        [JsonProperty("href")]
        public string Href { get; set;}

        [JsonProperty("text")]
        public Text Text { get; set;}

        [JsonProperty("annotations")]
        public Annotations Annotations { get; set;}
    }

    public partial class Annotations
    {
        [JsonProperty("bold")]
        public bool Bold { get; set; }

        [JsonProperty("italic")]
        public bool Italic { get; set; }

        [JsonProperty("strikethrough")]
        public bool Strikethrough { get; set; }

        [JsonProperty("underline")]
        public bool Underline { get; set; }

        [JsonProperty("code")]
        public bool Code { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }
    }

    public partial class CheckboxProperty : NotionProperty
    {

        [JsonProperty("checkbox")]
        public bool Checkbox { get; set; }
    }

    public partial class SelectProperty : NotionProperty
    {

        [JsonProperty("select")]
        public Select Select { get; set; }
    }

    public partial class Select
    {
        [JsonProperty("options")]
        public Option[] Options { get; set; }
    }

    public partial class Option
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }
    }

    public partial class RelationProperty : NotionProperty
    {

        [JsonProperty("relation")]
        public Relation Relation { get; set; }
    }

    public partial class Relation
    {
        [JsonProperty("database")]
        public Guid Database { get; set; }

        [JsonProperty("synced_property_name")]
        public object SyncedPropertyName { get; set; }
    }

    public partial class EmptyObject
    {
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Martijn.Extensions.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ApiLibs.NotionRest
{
    public interface INotionProperty<T>
    {
        public void Set(T input);
        public T Get();
    }

    [JsonConverter(typeof(NotionPropertyConverter))]
    public abstract class NotionProperty
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class NumberProperty : NotionProperty, INotionProperty<double?>
    {
        [JsonProperty("number")]
        public double? Number { get; set; }

        public NumberProperty()
        {
            Type = "number";
        }

        public double? Get()
        {
            return Number;
        }

        public void Set(double? input)
        {
            Number = input;
        }
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
                "title" => new TitleProperty(),
                "rich_text" => new RichTextProperty(),
                "number" => new NumberProperty(),
                "checkbox" => new CheckboxProperty(),
                "select" => new SelectPropertyValue(),
                "multi_select" => new MultiSelectProperty(),
                "relation" => new RelationProperty(),
                "url" => new UrlProperty(),
                "date" => new DateProperty(),
                "email" => new EmailProperty(),
                "phone_number" => new PhoneProperty(),
                "last_edited_time" => new LastEditedTimeProperty(),
                _ => throw new ArgumentOutOfRangeException("Cannot convert type " + type + jObject.ToString())
            };

            serializer.Populate(jObject.CreateReader(), result);

            if (result is DateProperty dprop)
            {
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

    public class TitleProperty : NotionProperty, INotionProperty<string>
    {
        public TitleProperty()
        {
            Title = new List<RichText>();
        }

        [JsonProperty("title")]
        public List<RichText> Title { get; set; }

        public string Get() => ToPlainText();

        public void Set(string input)
        {
            Title = new List<RichText>{
                new RichText {
                    PlainText = input,
                    Text = new Text() {
                        Content = input
                    }
                }
            };
        }

        public string ToPlainText() => Title?.Select(i => i.PlainText).Combine("");
    }

    public class UrlProperty : NotionProperty, INotionProperty<string>
    {

        [JsonProperty("url")]
        public string Url { get; set; }

        public string Get() => Url;

        public void Set(string input)
        {
            Url = input;
        }
    }

    public class LastEditedTimeProperty : NotionProperty
    {

        [JsonProperty("last_edited_time")]
        public DateTime LastEdited { get; set; }
    }

    public class EmailProperty : NotionProperty, INotionProperty<string>
    {

        [JsonProperty("email")]
        public string Email { get; set; }

        public string Get() => Email;

        public void Set(string input)
        {
            Email = input;
        }
    }

    public class PhoneProperty : NotionProperty, INotionProperty<string>
    {

        [JsonProperty("phone_number")]
        public string PhoneNumber { get; set; }

        public string Get() => PhoneNumber;

        public void Set(string input)
        {
            PhoneNumber = input;
        }
    }

    [JsonConverter(typeof(DatePropertyConverter))]
    public class DateProperty : NotionProperty, INotionProperty<DateTime?>
    {
        [JsonProperty("start")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? Start { get; set; }

        [JsonProperty("end")]
        public DateTime? End { get; set; }

        public DateTime? Get() => Start;

        public void Set(DateTime? input)
        {
            Start = input;
        }
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

        public static DateTime? GetDateTime(string date)
        {
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

    public partial class RichTextProperty : NotionProperty, INotionProperty<string>
    {
        public RichTextProperty()
        {
            RichText = new List<RichText>();
            Type = "rich_text";
        }

        [JsonProperty("rich_text")]
        public List<RichText> RichText { get; set; }

        public string Get() => ToPlainText();

        public void Set(string input)
        {
            RichText = new List<RichText>{
                new RichText {
                    PlainText = input,
                    Text = new Text() {
                        Content = input
                    }
                }
            };
        }

        public string ToPlainText() => RichText?.Select(i => i.PlainText).Combine("");
    }

    public class RichText
    {
        [JsonProperty("plain_text")]
        public string PlainText { get; set; }

        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("text")]
        public Text Text { get; set; }

        [JsonProperty("annotations")]
        public Annotations Annotations { get; set; }
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

    public partial class CheckboxProperty : NotionProperty, INotionProperty<bool>
    {
        public CheckboxProperty()
        {
            Type = "checkbox";
        }

        [JsonProperty("checkbox")]
        public bool Checkbox { get; set; }

        public bool Get()
        {
            return Checkbox;
        }

        public void Set(bool input)
        {
            Checkbox = input;
        }
    }

    public partial class SelectPropertyValue : NotionProperty, INotionProperty<Option>
    {
        public SelectPropertyValue()
        {
            Type = "select";
        }

        [JsonProperty("select")]
        public Option Select { get; set; }

        public Option Get()
        {
            return Select;
        }

        public void Set(Option input)
        {
            Select = input;
        }
    }

    public partial class MultiSelectProperty : NotionProperty, INotionProperty<List<Option>>
    {
        public MultiSelectProperty()
        {
            Type = "multi_select";
        }

        [JsonProperty("multi_select")]
        public List<Option> MultiSelect { get; set; }

        public List<Option> Get()
        {
            return MultiSelect;
        }

        public void Set(List<Option> input)
        {
            MultiSelect = input;
        }
    }

    public partial class SelectProperty : NotionProperty
    {
        public SelectProperty()
        {
            Type = "select";
        }

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
        public string Id { get; set; }

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


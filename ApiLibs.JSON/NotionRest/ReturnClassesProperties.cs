using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Martijn.Extensions.JSON.Linq;
using System.Text.Json.Serialization;
using Martijn.Extensions.JSON.Compat;
using System.Text.Json;
using Martijn.Extensions.JSON.Compat;

namespace ApiLibs.JSON.NotionRest
{
    public interface INotionProperty<T>
    {
        public void Set(T input);
        public T Get();
    }

    [JsonConverter(typeof(NotionPropertyConverter))]
    public abstract class NotionProperty
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class NumberProperty : NotionProperty, INotionProperty<double?>
    {
        [JsonPropertyName("number")]
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

        // Newtonsoft passed JSON null to the converter too (which then threw: there is no "null" type).
        public override bool HandleNull => true;

        public override NotionProperty Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
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

            NotionProperty result = type switch
            {
                "title" => new TitleProperty(),
                "rich_text" => new RichTextProperty(),
                "number" => new NumberProperty(),
                "checkbox" => new CheckboxProperty(),
                "select" => new SelectPropertyValue(),
                "multi_select" => new MultiSelectProperty(),
                "files" => new FilesProperty(),
                "relation" => new RelationProperty(),
                "url" => new UrlProperty(),
                "date" => new DateProperty(),
                "email" => new EmailProperty(),
                "phone_number" => new PhoneProperty(),
                "last_edited_time" => new LastEditedTimeProperty(),
                _ => throw new ArgumentOutOfRangeException("Cannot convert type " + type + jObject.ToString())
            };

            NewtonsoftPopulate.Populate(jObject, result);

            if (result is DateProperty dprop)
            {
                // Was jObject["date"].CreateReader(): a missing "date" threw a NullReferenceException.
                if (!jObject.TryGetProperty("date", out var date))
                {
                    throw new NullReferenceException();
                }
                result = DatePropertyConverter.ReadInto(date, dprop);
            }


            return result;
        }

        /// <summary>The Newtonsoft version had CanWrite => false: properties are written member by member.</summary>
        public override void Write(Utf8JsonWriter writer, NotionProperty value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue();
                return;
            }
            NewtonsoftPopulate.WriteDefault(writer, value, options);
        }
    }

    public class TitleProperty : NotionProperty, INotionProperty<string>
    {
        public TitleProperty()
        {
            Title = new List<RichText>();
        }

        [JsonPropertyName("title"), NullValueHandling(NullHandling.Include)]
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

        [JsonPropertyName("url")]
        public string Url { get; set; }

        public string Get() => Url;

        public void Set(string input)
        {
            Url = input;
        }
    }

    public class LastEditedTimeProperty : NotionProperty
    {

        [JsonPropertyName("last_edited_time")]
        public DateTime LastEdited { get; set; }
    }

    public class EmailProperty : NotionProperty, INotionProperty<string>
    {

        [JsonPropertyName("email")]
        public string Email { get; set; }

        public string Get() => Email;

        public void Set(string input)
        {
            Email = input;
        }
    }

    public class PhoneProperty : NotionProperty, INotionProperty<string>
    {

        [JsonPropertyName("phone_number")]
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
        [JsonPropertyName("start")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? Start { get; set; }

        [JsonPropertyName("end")]
        public DateTime? End { get; set; }

        public DateTime? Get() => Start;

        public void Set(DateTime? input)
        {
            Start = input;
        }
    }

    public class DateTimeConverter : JsonConverter<DateTime?>
    {
        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var document = JsonDocument.ParseValue(ref reader);
            // Was JToken.ReadFrom(reader).Value<string>(): an ISO date-time had been parsed to a DateTime
            // and came back as "MM/dd/yyyy HH:mm:ss", which GetDateTime cannot parse.
            string val = NewtonsoftPopulate.ValueAsString(document.RootElement);
            return DatePropertyConverter.GetDateTime(val);
        }

        /// <summary>The Newtonsoft version had CanWrite => false: written like any DateTime?.</summary>
        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            // Not options.GetConverter(typeof(DateTime?)): when this converter is applied through the
            // options, that returns this converter again.
            if (value == null)
            {
                writer.WriteNullValue();
            }
            else
            {
                writer.WriteStringValue(NewtonsoftDates.Format(value.Value));
            }
        }
    }

    public class DatePropertyConverter : JsonConverter<DateProperty>
    {
        // Newtonsoft passed JSON null to the converter too.
        public override bool HandleNull => true;

        /// <summary>
        /// Newtonsoft passed the property's existing value here; System.Text.Json cannot, so this
        /// behaves as when there was none (Populate throws on a null target).
        /// </summary>
        public override DateProperty Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var document = JsonDocument.ParseValue(ref reader);
            return ReadInto(document.RootElement.Clone(), null);
        }

        internal static DateProperty ReadInto(JsonElement jObject, DateProperty existingValue)
        {
            NewtonsoftPopulate.Populate(jObject, existingValue);

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

        /// <summary>The Newtonsoft version had CanWrite => false: written member by member.</summary>
        public override void Write(Utf8JsonWriter writer, DateProperty value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue();
                return;
            }
            NewtonsoftPopulate.WriteDefault(writer, value, options);
        }
    }

    public partial class RichTextProperty : NotionProperty, INotionProperty<string>
    {
        public RichTextProperty()
        {
            RichText = new List<RichText>();
            Type = "rich_text";
        }

        [JsonPropertyName("rich_text")]
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
        [JsonPropertyName("plain_text")]
        public string PlainText { get; set; }

        [JsonPropertyName("href")]
        public string Href { get; set; }

        [JsonPropertyName("text")]
        public Text Text { get; set; }

        [JsonPropertyName("annotations")]
        public Annotations Annotations { get; set; }
    }

    public partial class Annotations
    {
        [JsonPropertyName("bold")]
        public bool Bold { get; set; }

        [JsonPropertyName("italic")]
        public bool Italic { get; set; }

        [JsonPropertyName("strikethrough")]
        public bool Strikethrough { get; set; }

        [JsonPropertyName("underline")]
        public bool Underline { get; set; }

        [JsonPropertyName("code")]
        public bool Code { get; set; }

        [JsonPropertyName("color")]
        public string Color { get; set; }
    }

    public partial class CheckboxProperty : NotionProperty, INotionProperty<bool>
    {
        public CheckboxProperty()
        {
            Type = "checkbox";
        }

        [JsonPropertyName("checkbox"), NullValueHandling(NullHandling.Include)]
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

        [JsonPropertyName("select"), NullValueHandling(NullHandling.Include)]
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

    public partial class FilesProperty : NotionProperty, INotionProperty<List<NotionFile>>
    {
        public FilesProperty()
        {
            Type = "files";
        }

        [JsonPropertyName("files")]
        public List<NotionFileWrapper> Files { get; set; }

        public List<NotionFile> Get()
        {
            return Files.Select(i => i.Value).ToList();
        }

        public void Set(List<NotionFile> input)
        {
            Files = input.Select(i => new NotionFileWrapper
            {
                Value = i
            }).ToList();
        }
    }

    public partial class MultiSelectProperty : NotionProperty, INotionProperty<List<Option>>
    {
        public MultiSelectProperty()
        {
            Type = "multi_select";
        }

        [JsonPropertyName("multi_select")]
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

        [JsonPropertyName("select"), NullValueHandling(NullHandling.Include)]
        public Select Select { get; set; }
    }

    public partial class Select
    {
        [JsonPropertyName("options")]
        public Option[] Options { get; set; }
    }

    public partial class Option
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("color")]
        public string Color { get; set; }
    }

    public partial class RelationProperty : NotionProperty
    {

        [JsonPropertyName("relation")]
        public Relation Relation { get; set; }
    }

    public partial class Relation
    {
        [JsonPropertyName("database")]
        public Guid Database { get; set; }

        [JsonPropertyName("synced_property_name")]
        public object SyncedPropertyName { get; set; }
    }

    public partial class EmptyObject
    {
    }
}


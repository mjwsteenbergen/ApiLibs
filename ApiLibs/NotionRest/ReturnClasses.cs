using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiLibs.General;
using Martijn.Extensions.AsyncLinq;
using Newtonsoft.Json;

namespace ApiLibs.NotionRest
{
    public class NotionObject {

    }

    public class NotionDatabase : ObjectSearcher<NotionRestService>
    {
        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("created_time")]
        public DateTimeOffset CreatedTime { get; set; }

        [JsonProperty("last_edited_time")]
        public DateTimeOffset LastEditedTime { get; set; }

        [JsonProperty("parent")]
        public Parent Parent { get; set; }

        [JsonProperty("icon")]
        public Icon Icon { get; set; }

        [JsonProperty("cover")]
        public File Cover { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("title")]
        public Title[] Title { get; set; }

        // [JsonProperty("properties")]
        // public Dictionary<string, NotionProperty> Properties { get; set; }

        public Task<List<Page>> GetPages() => this.Service.QueryDatabase(this.Id).ToList();
    }

    public partial class Icon
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("emoji")]
        public string Emoji { get; set; }
    }

    public partial class Parent
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("page_id")]
        public Guid PageId { get; set; }
    }

    public partial class Formula
    {
        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public partial class LastOrdered
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("date")]
        public EmptyObject Date { get; set; }
    }

    public partial class NumberOfMeals
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("rollup")]
        public Rollup Rollup { get; set; }
    }

    public partial class Rollup
    {
        [JsonProperty("rollup_property_name")]
        public string RollupPropertyName { get; set; }

        [JsonProperty("relation_property_name")]
        public string RelationPropertyName { get; set; }

        [JsonProperty("rollup_property_id")]
        public string RollupPropertyId { get; set; }

        [JsonProperty("relation_property_id")]
        public string RelationPropertyId { get; set; }

        [JsonProperty("function")]
        public string Function { get; set; }
    }

    public partial class Photo
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("files")]
        public EmptyObject Files { get; set; }
    }

    public partial class Price
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("number")]
        public Number Number { get; set; }
    }

    public partial class Number
    {
        [JsonProperty("format")]
        public string Format { get; set; }
    }

    public partial class StoreAvailability
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("multi_select")]
        public MultiSelect MultiSelect { get; set; }
    }

    public partial class MultiSelect
    {
        [JsonProperty("options")]
        public Option[][] Options { get; set; }
    }

    public partial class The1
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("people")]
        public EmptyObject People { get; set; }
    }

    public partial class Title
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("text")]
        public Text Text { get; set; }

        [JsonProperty("annotations")]
        public Annotations Annotations { get; set; }

        [JsonProperty("plain_text")]
        public string PlainText { get; set; }

        [JsonProperty("href")]
        public object Href { get; set; }
    }

    public partial class Text
    {
        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("link")]
        public object Link { get; set; }
    }

    public class File
    {
        [JsonProperty("external")]
        public External External { get; set;}

        [JsonProperty("url")]
        public Uri Url { get; set;}

        [JsonProperty("expiry_time")]
        public DateTime? ExpiryTime { get; set;}
    }

    public class External {
        [JsonProperty("url")]
        public Uri Url { get; set;}
    }

    // public partial class NotionDatabase
    // {
    //     public static NotionDatabase FromJson(string json) => JsonConvert.DeserializeObject<NotionDatabase>(json, QuickType.Converter.Settings);
    // }

    // public static class Serialize
    // {
    //     public static string ToJson(this NotionDatabase self) => JsonConvert.SerializeObject(self, QuickType.Converter.Settings);
    // }

    // internal static class Converter
    // {
    //     public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
    //     {
    //         MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
    //         DateParseHandling = DateParseHandling.None,
    //         Converters = {
    //             new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
    //         },
    //     };
    // }

    public class NotionList<T> {
        [JsonProperty("object")]
        public string Object { get; set;}

        [JsonProperty("results")]
        public List<T> Results { get; set;}

        [JsonProperty("has_more")]
        public bool HasMore { get; set;}

        [JsonProperty("next_cursor")]
        public string NextCursor { get; set;}
    }

    public partial class Page
    {
        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("created_time")]
        public DateTimeOffset CreatedTime { get; set; }

        [JsonProperty("last_edited_time")]
        public DateTimeOffset LastEditedTime { get; set; }

        [JsonProperty("parent")]
        public Parent Parent { get; set; }

        [JsonProperty("archived")]
        public bool Archived { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("icon")]
        public Icon Icon { get; set; }

        [JsonProperty("cover")]
        public File Cover { get; set; }

        [JsonProperty("properties")]
        public Dictionary<string, NotionProperty> Properties { get; set; }
    }

    public class Page<T> : Page where T : PageProps, new()
    {
        public PageProps Props { get { return new T().With(this); } }
    }

    public abstract class PageProps {

        public Dictionary<string, NotionProperty> Properties { get; private set; }

        public Page Page { get; set; }

        public PageProps() {}
        public PageProps(Page p) {
            With(p);
        }

        internal PageProps With(Page page)
        {
            Properties = page.Properties ?? new Dictionary<string, NotionProperty>();
            Page = page;
            return this;
        }

    }

    public partial class Parent
    {

        [JsonProperty("database_id")]
        public Guid DatabaseId { get; set; }
    }

    public partial class Photo
    {
        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("mime_type")]
        public string MimeType { get; set; }
    }

    public partial class Person
    {
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}

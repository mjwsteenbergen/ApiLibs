using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiLibs.JSON.General;
using Martijn.Extensions.JSON.AsyncLinq;
using Martijn.Extensions.JSON.Generics;
using System.Text.Json.Serialization;
using System.Linq;

namespace ApiLibs.JSON.NotionRest
{
    public class NotionObject {

    }

    public class NotionDatabase : ObjectSearcher<NotionRestService>
    {
        [JsonPropertyName("object")]
        public string Object { get; set; }

        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("created_time")]
        public DateTimeOffset CreatedTime { get; set; }

        [JsonPropertyName("last_edited_time")]
        public DateTimeOffset LastEditedTime { get; set; }

        [JsonPropertyName("parent")]
        public Parent Parent { get; set; }

        [JsonPropertyName("icon")]
        public Icon Icon { get; set; }

        [JsonPropertyName("cover")]
        public HostedNotionFile Cover { get; set; }

        [JsonPropertyName("url")]
        public Uri Url { get; set; }

        [JsonPropertyName("title")]
        public Title[] Title { get; set; }

        [JsonPropertyName("properties")]
        public Dictionary<string, NotionDatabaseProperty> Properties { get; set; }

        [JsonIgnore]
        public List<NotionDatabaseProperty> PropertyList => this.Properties.Values.ToList();

        public Task<List<Page>> GetPages() => Service.QueryDatabase(this.Id).ToList();

        public Task<Page> CreatePage(Dictionary<string, NotionProperty> props) => Service.Page.Create(this, props);
    }

    public partial class Icon
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("emoji")]
        public string Emoji { get; set; }
    }

    public partial class Parent
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("page_id")]
        public Guid? PageId { get; set; }
    }

    public partial class Formula
    {
        [JsonPropertyName("value")]
        public string Value { get; set; }
    }

    public partial class LastOrdered
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("date")]
        public EmptyObject Date { get; set; }
    }

    public partial class NumberOfMeals
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("rollup")]
        public Rollup Rollup { get; set; }
    }

    public partial class Rollup
    {
        [JsonPropertyName("rollup_property_name")]
        public string RollupPropertyName { get; set; }

        [JsonPropertyName("relation_property_name")]
        public string RelationPropertyName { get; set; }

        [JsonPropertyName("rollup_property_id")]
        public string RollupPropertyId { get; set; }

        [JsonPropertyName("relation_property_id")]
        public string RelationPropertyId { get; set; }

        [JsonPropertyName("function")]
        public string Function { get; set; }
    }

    public partial class Photo
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("files")]
        public EmptyObject Files { get; set; }
    }

    public partial class StoreAvailability
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("multi_select")]
        public MultiSelect MultiSelect { get; set; }
    }

    public partial class MultiSelect
    {
        [JsonPropertyName("options")]
        public Option[][] Options { get; set; }
    }

    public partial class The1
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("people")]
        public EmptyObject People { get; set; }
    }

    public partial class Title
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("text")]
        public Text Text { get; set; }

        [JsonPropertyName("annotations")]
        public Annotations Annotations { get; set; }

        [JsonPropertyName("plain_text")]
        public string PlainText { get; set; }

        [JsonPropertyName("href")]
        public object Href { get; set; }
    }

    public partial class Text
    {
        [JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonPropertyName("link")]
        public TextLink Link { get; set; }
    }

    public class TextLink {

        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
    
    
    

    public class NotionList<T> {
        [JsonPropertyName("object")]
        public string Object { get; set;}

        [JsonPropertyName("results")]
        public List<T> Results { get; set;}

        [JsonPropertyName("has_more")]
        public bool HasMore { get; set;}

        [JsonPropertyName("next_cursor")]
        public string NextCursor { get; set;}
    }

    public partial class Page
    {
        [JsonPropertyName("object")]
        public string Object { get; set; }

        [JsonPropertyName("id")]
        public Guid? Id { get; set; }

        [JsonPropertyName("created_time")]
        public DateTimeOffset? CreatedTime { get; set; }

        [JsonPropertyName("last_edited_time")]
        public DateTimeOffset? LastEditedTime { get; set; }

        [JsonPropertyName("parent")]
        public Parent Parent { get; set; }

        [JsonPropertyName("archived")]
        public bool? Archived { get; set; }

        [JsonPropertyName("url")]
        public Uri Url { get; set; }

        [JsonPropertyName("icon")]
        public Icon Icon { get; set; }

        [JsonPropertyName("cover")]
        public NotionFileWrapper Cover { get; set; }

        [JsonPropertyName("properties")]
        public Dictionary<string, NotionProperty> Properties { get; set; }

        public Page<T> WithProperties<T>() where T : PageProps, new()
        {
            return new Page<T>(this);
        }
    }

    public class Page<T> : Page where T : PageProps, new()
    {
        public Page(Page page) {
            page.CopyPropertiesTo(this);
            Props = new T().With(this) as T;
        }

        [JsonIgnore]
        public T Props { get; set; }

        
    }
    

    public abstract class PageProps {

        public PageProps() {
            Properties = new Dictionary<string, NotionProperty>();
        }

        [JsonIgnore]
        public Dictionary<string, NotionProperty> Properties { get; private set; }

        [JsonIgnore]
        public Page Page { get; set; }

        [JsonIgnore]
        private readonly Dictionary<string, NotionProperty> Updates = new();

        [JsonPropertyName("id")]
        public string Id { get { return Page?.Id?.ToString()?.Replace("-", ""); } }

        public PageProps With(Page page)
        {
            Properties = page.Properties ?? new Dictionary<string, NotionProperty>();
            Page = page;
            return this;
        }

        public T GetProp<T>(string name) where T : NotionProperty
        {
            return (Properties.ContainsKey(name) ? Properties[name] : default(T)) as T;
        }

        protected Y Get<T,Y>(string name) where T : NotionProperty, INotionProperty<Y>
        {
            var res = GetProp<T>(name);

            if(res == default(T)) {
                return default;
            }
            return res.Get();
        }

        protected void Set<T, Y>(string name, Y value) where T : NotionProperty, INotionProperty<Y>, new()
        {
            var prop = GetProp<T>(name);
            if(prop == null)
            {
                Properties.Add(name, new T());
                prop = GetProp<T>(name);
            }
            prop.Set(value);

            Updates.Remove(name);
            Updates.Add(name, prop);
        }

        public Task Update(NotionRestService service) => service.Page.Update(Page.Id ?? throw new NullReferenceException(), new Page()
        {
            Properties = Updates
        });
    }

    public partial class Parent
    {

        [JsonPropertyName("database_id")]
        public Guid? DatabaseId { get; set; }
    }

    public partial class Photo
    {
        [JsonPropertyName("url")]
        public Uri Url { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("mime_type")]
        public string MimeType { get; set; }
    }

    public partial class Person
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }
    }
}

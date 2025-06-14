using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiLibs.General;
using Martijn.Extensions.AsyncLinq;
using Martijn.Extensions.Generics;
using Newtonsoft.Json;
using System.Linq;

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
        public HostedNotionFile Cover { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("title")]
        public Title[] Title { get; set; }

        [JsonProperty("properties")]
        public Dictionary<string, NotionDatabaseProperty> Properties { get; set; }

        [JsonIgnore]
        public List<NotionDatabaseProperty> PropertyList => this.Properties.Values.ToList();

        public Task<List<Page>> GetPages() => Service.QueryDatabase(this.Id).ToList();

        public Task<Page> CreatePage(Dictionary<string, NotionProperty> props) => Service.Page.Create(this, props);
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
        public Guid? PageId { get; set; }
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
        public TextLink Link { get; set; }
    }

    public class TextLink {

        [JsonProperty("url")]
        public string Url { get; set; }
    }
    
    
    

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
        public Guid? Id { get; set; }

        [JsonProperty("created_time")]
        public DateTimeOffset? CreatedTime { get; set; }

        [JsonProperty("last_edited_time")]
        public DateTimeOffset? LastEditedTime { get; set; }

        [JsonProperty("parent")]
        public Parent Parent { get; set; }

        [JsonProperty("archived")]
        public bool? Archived { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("icon")]
        public Icon Icon { get; set; }

        [JsonProperty("cover")]
        public NotionFileWrapper Cover { get; set; }

        [JsonProperty("properties")]
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

        [JsonProperty("id")]
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

        [JsonProperty("database_id")]
        public Guid? DatabaseId { get; set; }
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

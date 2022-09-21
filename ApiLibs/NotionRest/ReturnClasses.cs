using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiLibs.General;
using Martijn.Extensions.AsyncLinq;
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
        public File Cover { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("title")]
        public Title[] Title { get; set; }

        // [JsonProperty("properties")]
        // public Dictionary<string, NotionProperty> Properties { get; set; }

        public Task<List<Page>> GetPages() => Service.QueryDatabase(this.Id).ToList();

        public Task<Page> CreatePage(Dictionary<string, NotionProperty> props) => Service.CreatePage(this, props);
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

        public Page<T> WithProperties<T>() where T : PageProps, new()
        {
            return new Page<T>(this);
        }
    }

    public class Page<T> : Page where T : PageProps, new()
    {
        public Page(Page page) {
            CopyPropertiesTo(page, this);
            Props = new T().With(this) as T;
        }

        [JsonIgnore]
        public T Props { get; set; }

        private static void CopyPropertiesTo<T1, TU>(T1 source, TU dest)
        {
            var sourceProps = typeof(T1).GetProperties().Where(x => x.CanRead).ToList();
            var destProps = typeof(TU).GetProperties()
                    .Where(x => x.CanWrite)
                    .ToList();

            foreach (var sourceProp in sourceProps)
            {
                if (destProps.Any(x => x.Name == sourceProp.Name))
                {
                    var p = destProps.First(x => x.Name == sourceProp.Name);
                    if (p.CanWrite)
                    { // check if the property can be set or no.
                        p.SetValue(dest, sourceProp.GetValue(source, null), null);
                    }
                }

            }

        }
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
        private readonly HashSet<NotionProperty> Updates = new();

        [JsonProperty("id")]
        public string Id { get { return Page.Id.ToString()?.Replace("-", ""); } }

        public PageProps With(Page page)
        {
            Properties = page.Properties ?? new Dictionary<string, NotionProperty>();
            Page = page;
            return this;
        }

        public T GetProp<T>(string name) where T : NotionProperty
        {
            return (Properties.ContainsKey(name) ? Properties[name] : null) as T;
        }

        protected Y Get<T,Y>(string name) where T : NotionProperty, INotionProperty<Y>
        {
            return GetProp<T>(name).Get();
        }

        protected void Set<T, Y>(T prop, Y value) where T : NotionProperty, INotionProperty<Y>
        {
            prop.Set(value);
            Updates.Add(prop);
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
            Updates.Add(prop);
        }

        protected Task Update(NotionRestService service) => service.UpdatePage(Page.Id, new Page()
        {
            Properties = Updates.ToDictionary(i => i.Name)
        });

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

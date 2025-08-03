using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace ApiLibs.NotionRest
{
    public interface INotionBlock : WithType
    {

    }

    public abstract class NotionBlock : INotionBlock
    {
        [JsonProperty("has_children")]
        public bool? HasChildren { get; set; }

        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("id")]
        public Guid? Id { get; set; }
        public string Type { get; set; }
    }

    public class ForceDefaultConverter : JsonConverter
    {
        public override bool CanRead => false;
        public override bool CanWrite => false;

        public override bool CanConvert(Type objectType)
        {
            throw new InvalidOperationException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new InvalidOperationException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new InvalidOperationException();
        }
    }

    [JsonConverter(typeof(NotionBlockConverter))]
    public class NotionBlockWrapper : NotionUnionTypeWrapper<INotionBlock> { }

    public class NotionBlockConverter : NotionObjectWithTypeConverter<INotionBlock, NotionBlockWrapper>
    {
        public override INotionBlock Read(string type, JToken token)
        {
            return type switch
            {
                "paragraph" => new Paragraph(),
                "heading_1" => new Heading1(),
                "heading_2" => new Heading2(),
                "heading_3" => new Heading3(),
                "bulleted_list_item" => new BulletedListItem(),
                "numbered_list_item" => new NumberedListItem(),
                "to_do" => new ToDo(),
                "toggle" => new Toggle(),
                "child_page" => new ChildPage(),
                "child_database" => new ChildDatabase(),
                "embed" => new Embed(),
                "image" => new NotionImage(),
                "video" => new Video(),
                "file" => new FileBlock(),
                "pdf" => new Pdf(),
                "bookmark" => new Bookmark(),
                "callout" => new Callout(),
                "quote" => new Quote(),
                "equation" => new Equation(),
                "divider" => new Divider(),
                "table_of_contents" => new TableOfContents(),
                "column" => new Column(),
                "column_list" => new ColumnList(),
                "link_preview" => new LinkPreview(),
                "code" => new Code(),
                "unsupported" => new Unsupported(),
                _ => throw new ArgumentOutOfRangeException("Cannot convert type " + type + token.ToString())
            };
        }
    }
}
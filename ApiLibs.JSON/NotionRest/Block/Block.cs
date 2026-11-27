using System;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace ApiLibs.JSON.NotionRest
{
    public interface INotionBlock : WithType
    {

    }

    public abstract class NotionBlock : INotionBlock
    {
        [JsonPropertyName("has_children")]
        public bool? HasChildren { get; set; }

        [JsonPropertyName("object")]
        public string Object { get; set; }

        [JsonPropertyName("id")]
        public Guid? Id { get; set; }
        public string Type { get; set; }
    }

    /// <summary>
    /// The Newtonsoft version was a converter that neither read nor wrote (CanRead and CanWrite
    /// false) and threw from CanConvert. It is not used anywhere.
    /// </summary>
    public class ForceDefaultConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            throw new InvalidOperationException();
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            throw new InvalidOperationException();
        }
    }

    [JsonConverter(typeof(NotionBlockConverter))]
    public class NotionBlockWrapper : NotionUnionTypeWrapper<INotionBlock> { }

    public class NotionBlockConverter : NotionObjectWithTypeConverter<INotionBlock, NotionBlockWrapper>
    {
        public override INotionBlock Read(string type, JsonElement token)
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
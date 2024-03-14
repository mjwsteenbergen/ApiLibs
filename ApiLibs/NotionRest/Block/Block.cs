using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ApiLibs.NotionRest
{

    [JsonConverter(typeof(NotionBlockConverter))]
    public abstract class NotionBlock
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("has_children")]
        public bool HasChildren { get; set;}

        [JsonProperty("object")]
        public string Object { get; set;}

        [JsonProperty("id")]
        public Guid Id { get; set; }
    }

    public class NotionBlockConverter : JsonConverter<NotionBlock>
    {
        class NotionPropertyImp : NotionBlock { }

        public override NotionBlock ReadJson(JsonReader reader, Type objectType, NotionBlock existingValue, bool hasExistingValue, JsonSerializer serializer)
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

            NotionBlock result = type switch
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
                "image" => new Image(),
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
                _ => throw new ArgumentOutOfRangeException("Cannot convert type " + type + jObject.ToString())
            };


            serializer.Populate(jObject.CreateReader(), result);
            serializer.Populate(jObject[type].CreateReader(), result);
            return result;
        }

        public override bool CanWrite => true;

        public T Seria<T>(JsonWriter writer, JsonSerializer serializer, T value) {
            serializer.Serialize(writer, value);
            return value;
        }

        public override void WriteJson(JsonWriter writer, NotionBlock value, JsonSerializer serializer)
        {
            NotionBlock _ = value switch {
                Paragraph paragraph => Seria(writer, serializer, paragraph),
                Heading3 heading_3 => Seria(writer, serializer, heading_3),
                Heading2 heading_2 => Seria(writer, serializer, heading_2),
                Heading1 heading_1 => Seria(writer, serializer, heading_1),
                BulletedListItem bulleted_list_item => Seria(writer, serializer, bulleted_list_item),
                NumberedListItem numbered_list_item => Seria(writer, serializer, numbered_list_item),
                ToDo to_do => Seria(writer, serializer, to_do),
                Toggle toggle => Seria(writer, serializer, toggle),
                ChildPage child_page => Seria(writer, serializer, child_page),
                ChildDatabase child_database => Seria(writer, serializer, child_database),
                Embed embed => Seria(writer, serializer, embed),
                Image image => Seria(writer, serializer, image),
                Video video => Seria(writer, serializer, video),
                FileBlock file => Seria(writer, serializer, file),
                Pdf pdf => Seria(writer, serializer, pdf),
                Bookmark bookmark => Seria(writer, serializer, bookmark),
                Callout callout => Seria(writer, serializer, callout),
                Quote quote => Seria(writer, serializer, quote),
                Equation equation => Seria(writer, serializer, equation),
                Divider divider => Seria(writer, serializer, divider),
                TableOfContents table_of_contents => Seria(writer, serializer, table_of_contents),
                Column column => Seria(writer, serializer, column),
                ColumnList column_list => Seria(writer, serializer, column_list),
                LinkPreview link_preview => Seria(writer, serializer, link_preview),
                Code code => Seria(writer, serializer, code),
                Unsupported unsupported => Seria(writer, serializer, unsupported),
                _ => throw new Exception("Invalid option")
            };
        }
    }
}
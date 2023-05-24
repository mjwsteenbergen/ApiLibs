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

        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, NotionBlock value, JsonSerializer serializer)
        {
            throw new System.NotImplementedException();
        }
    }
}
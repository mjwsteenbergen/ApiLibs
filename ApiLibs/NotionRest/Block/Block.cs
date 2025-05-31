using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace ApiLibs.NotionRest
{

    [JsonConverter(typeof(NotionBlockConverter))]
    public abstract class NotionBlock
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("has_children")]
        public bool? HasChildren { get; set;}

        [JsonProperty("object")]
        public string Object { get; set;}

        [JsonProperty("id")]
        public Guid? Id { get; set; }
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

        public void Serialize(PropertyInfo info, NotionBlock value, JsonWriter writer)
        {
            var val = info.GetValue(value);

            if(val != null) 
            {
                var customAttributes = (JsonPropertyAttribute[])info.GetCustomAttributes(typeof(JsonPropertyAttribute), true);
                if (customAttributes.Length > 0)
                {
                    var myAttribute = customAttributes[0];
                    string propName = myAttribute.PropertyName;

                    if(!string.IsNullOrEmpty(propName)) {
                        writer.WritePropertyName(propName);
                    } else {
                        writer.WritePropertyName(info.Name);
                    }
                    // TODO: Do something with the value
                } else {
                    writer.WritePropertyName(info.Name);
                }

                writer.WriteRawValue(JsonConvert.SerializeObject(val, Formatting.None, new JsonSerializerSettings {
                    NullValueHandling = NullValueHandling.Ignore
                }));
            }
        }

        public override void WriteJson(JsonWriter writer, NotionBlock value, JsonSerializer serializer)
        {
            // var serializeObject = new {
            //     type = value.Type
            // };

            // var serializeObject = new Dictionary<string, dynamic> {
            //     { "type" , value.Type + "" },
            //     { value.Type, value }
            // };

           
            writer.WriteStartObject();

            var props = value.GetType().GetProperties();
            foreach(PropertyInfo info in props.Where(i => i.DeclaringType.FullName == "ApiLibs.NotionRest.NotionBlock"))
            {
                Serialize(info, value, writer);
            }

            writer.WritePropertyName(value.Type);
            writer.WriteStartObject();

            foreach(PropertyInfo info in props.Where(i => i.DeclaringType.FullName != "ApiLibs.NotionRest.NotionBlock"))
            {
                Serialize(info, value, writer);
            }

            writer.WriteEndObject();
            writer.WriteEndObject();

            // writer.WritePropertyName("type");
            // writer.WriteValue(value.Type);
            // writer.WritePropertyName(value.Type);
            // value.Type = null;



            // var s = JsonConvert.SerializeObject(value, new ForceDefaultConverter());
            // writer.WriteValue(new JRaw(s));
            // JsonSerializer.CreateDefault().Serialize()
            // JsonSerializer.Create().Serialize(writer, value);
            // serializeObject[value.Type] = value;

            // serializer.Serialize(writer, value);
        }
    }
}
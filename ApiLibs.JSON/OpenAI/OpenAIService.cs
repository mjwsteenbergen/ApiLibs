using System.Collections.Generic;
using System.Threading.Tasks;
using ApiLibs.JSON.NotionRest;
using System.Text.Json.Serialization;

namespace ApiLibs.JSON.OpenAI;

public class OpenAIService : RestSharpService
{
    public OpenAIService(string token) : base("https://api.openai.com/v1/")
    {
        AddStandardHeader("Authorization", $"Bearer {token}");
    }

    public Task<ChatCompletionResponse> ChatCompletion(string model, IEnumerable<OpenAIMessage> messages)
    {
        return MakeRequest<ChatCompletionResponse>("chat/completions", Call.POST, content: new
        {
            model,
            messages
        });
    }
}

public class OpenAIContent
{
    [JsonPropertyName("type")]
    public string Type { get; set; }
}

public class OpenAITextContent : OpenAIContent
{
    public OpenAITextContent(string text)
    {
        Type = "text";
        Text = text;
    }

    [JsonPropertyName("text")]
    public string Text { get; set; }
}

public class OpenAIUrlContent : OpenAIContent
{
    public class OpenAIImageUrl
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }
    }

    public OpenAIUrlContent(string url)
    {
        Type = "image_url";
        ImageUrl = new OpenAIImageUrl
        {
            Url = url
        };
    }

    [JsonPropertyName("image_url")]
    public OpenAIImageUrl ImageUrl { get; set; }
}

public abstract class OpenAIMessage
{
    [JsonPropertyName("role")]
    public string Role { get; set; }
}

public class DeveloperOpenAIMessage : OpenAIMessage
{
    public DeveloperOpenAIMessage()
    {
        Role = "developer";
    }

    [JsonPropertyName("content")]
    public string Content { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}

public class SystemOpenAIMessage : OpenAIMessage
{
    public SystemOpenAIMessage()
    {
        Role = "system";
    }

    [JsonPropertyName("content")]
    public string Content { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}

public class UserOpenAIMessage : OpenAIMessage
{
    public UserOpenAIMessage()
    {
        Role = "user";
    }

    [JsonPropertyName("content")]
    public List<OpenAIContent> Content { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ApiLibs.JSON.OpenAI;

public partial class ChatCompletionResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("object")]
    public string Object { get; set; }

    [JsonPropertyName("created")]
    public long Created { get; set; }

    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonPropertyName("choices")]
    public List<Choice> Choices { get; set; }

    [JsonPropertyName("usage")]
    public Usage Usage { get; set; }

    [JsonPropertyName("service_tier")]
    public string ServiceTier { get; set; }
}

public partial class Choice
{
    [JsonPropertyName("index")]
    public long Index { get; set; }

    [JsonPropertyName("message")]
    public Message Message { get; set; }

    [JsonPropertyName("logprobs")]
    public object Logprobs { get; set; }

    [JsonPropertyName("finish_reason")]
    public string FinishReason { get; set; }
}

public partial class Message
{
    [JsonPropertyName("role")]
    public string Role { get; set; }

    [JsonPropertyName("content")]
    public string Content { get; set; }

    [JsonPropertyName("refusal")]
    public object Refusal { get; set; }

    [JsonPropertyName("annotations")]
    public List<object> Annotations { get; set; }
}

public partial class Usage
{
    [JsonPropertyName("prompt_tokens")]
    public long PromptTokens { get; set; }

    [JsonPropertyName("completion_tokens")]
    public long CompletionTokens { get; set; }

    [JsonPropertyName("total_tokens")]
    public long TotalTokens { get; set; }

    [JsonPropertyName("prompt_tokens_details")]
    public PromptTokensDetails PromptTokensDetails { get; set; }

    [JsonPropertyName("completion_tokens_details")]
    public CompletionTokensDetails CompletionTokensDetails { get; set; }
}

public partial class CompletionTokensDetails
{
    [JsonPropertyName("reasoning_tokens")]
    public long ReasoningTokens { get; set; }

    [JsonPropertyName("audio_tokens")]
    public long AudioTokens { get; set; }

    [JsonPropertyName("accepted_prediction_tokens")]
    public long AcceptedPredictionTokens { get; set; }

    [JsonPropertyName("rejected_prediction_tokens")]
    public long RejectedPredictionTokens { get; set; }
}

public partial class PromptTokensDetails
{
    [JsonPropertyName("cached_tokens")]
    public long CachedTokens { get; set; }

    [JsonPropertyName("audio_tokens")]
    public long AudioTokens { get; set; }
}

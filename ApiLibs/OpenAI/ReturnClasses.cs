using System.Collections.Generic;
using Newtonsoft.Json;

namespace ApiLibs.OpenAI;

public partial class ChatCompletionResponse
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("object")]
    public string Object { get; set; }

    [JsonProperty("created")]
    public long Created { get; set; }

    [JsonProperty("model")]
    public string Model { get; set; }

    [JsonProperty("choices")]
    public List<Choice> Choices { get; set; }

    [JsonProperty("usage")]
    public Usage Usage { get; set; }

    [JsonProperty("service_tier")]
    public string ServiceTier { get; set; }
}

public partial class Choice
{
    [JsonProperty("index")]
    public long Index { get; set; }

    [JsonProperty("message")]
    public Message Message { get; set; }

    [JsonProperty("logprobs")]
    public object Logprobs { get; set; }

    [JsonProperty("finish_reason")]
    public string FinishReason { get; set; }
}

public partial class Message
{
    [JsonProperty("role")]
    public string Role { get; set; }

    [JsonProperty("content")]
    public string Content { get; set; }

    [JsonProperty("refusal")]
    public object Refusal { get; set; }

    [JsonProperty("annotations")]
    public List<object> Annotations { get; set; }
}

public partial class Usage
{
    [JsonProperty("prompt_tokens")]
    public long PromptTokens { get; set; }

    [JsonProperty("completion_tokens")]
    public long CompletionTokens { get; set; }

    [JsonProperty("total_tokens")]
    public long TotalTokens { get; set; }

    [JsonProperty("prompt_tokens_details")]
    public PromptTokensDetails PromptTokensDetails { get; set; }

    [JsonProperty("completion_tokens_details")]
    public CompletionTokensDetails CompletionTokensDetails { get; set; }
}

public partial class CompletionTokensDetails
{
    [JsonProperty("reasoning_tokens")]
    public long ReasoningTokens { get; set; }

    [JsonProperty("audio_tokens")]
    public long AudioTokens { get; set; }

    [JsonProperty("accepted_prediction_tokens")]
    public long AcceptedPredictionTokens { get; set; }

    [JsonProperty("rejected_prediction_tokens")]
    public long RejectedPredictionTokens { get; set; }
}

public partial class PromptTokensDetails
{
    [JsonProperty("cached_tokens")]
    public long CachedTokens { get; set; }

    [JsonProperty("audio_tokens")]
    public long AudioTokens { get; set; }
}

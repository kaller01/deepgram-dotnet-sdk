﻿namespace Deepgram.Models.Manage.v1;

public record UsageModel
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("language")]
    public string? Language { get; set; }

    [JsonPropertyName("version")]
    public string? Version { get; set; }

    [JsonPropertyName("model_id")]
    public string? ModelId { get; set; }
}


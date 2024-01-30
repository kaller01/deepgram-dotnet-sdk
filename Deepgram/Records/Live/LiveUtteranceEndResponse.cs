﻿namespace Deepgram.Records.Live;
public class LiveUtteranceEndResponse
{
    [JsonPropertyName("type")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public LiveType Type { get; set; } = LiveType.UtteranceEnd;
    [JsonPropertyName("channel_index")]
    public int[]? Channel { get; set; }

    [JsonPropertyName("last_word_end")]
    public double? LastWordEnd { get; set; }
}

﻿namespace Deepgram.Models.Manage.v1;
public record GetProjectUsageRequestsResponse
{
    [JsonPropertyName("page")]
    public int? Page { get; set; }

    [JsonPropertyName("limit")]
    public int? Limit { get; set; }

    [JsonPropertyName("requests")]
    public IReadOnlyList<GetProjectUsageRequestResponse>? Requests { get; set; }
}

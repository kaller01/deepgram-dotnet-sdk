﻿namespace Deepgram.Models.Manage.v1;

public record GetProjectInvitesResponse
{
    /// <summary>
    /// ReadOnlyList of project invites <see cref="Invite"/>
    /// </summary>
    [JsonPropertyName("invites")]
    public IReadOnlyList<Invite>? Invites { get; set; }
}

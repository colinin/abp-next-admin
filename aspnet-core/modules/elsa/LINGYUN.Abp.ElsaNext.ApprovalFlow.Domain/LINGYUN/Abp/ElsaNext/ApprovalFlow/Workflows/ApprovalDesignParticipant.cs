using LINGYUN.Abp.ElsaNext.ApprovalFlow.Participants;
using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Workflows;

public sealed class ApprovalDesignParticipant
{
    public string Provider { get; set; } = ParticipantConsts.DefaultProvider;

    public string Type { get; set; } = ParticipantConsts.UserType;

    public string Id { get; set; } = "";

    public string? DisplayName { get; set; }
}

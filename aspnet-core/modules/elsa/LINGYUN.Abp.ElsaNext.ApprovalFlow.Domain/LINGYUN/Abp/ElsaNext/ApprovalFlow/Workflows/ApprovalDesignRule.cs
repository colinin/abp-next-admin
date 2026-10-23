using LINGYUN.Abp.ElsaNext.ApprovalFlow.Participants;
using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Workflows;

public sealed class ApprovalDesignRule
{
    public string Kind { get; set; } = "";

    public List<ApprovalDesignParticipant> Participants { get; set; } = [];

    public int? Level { get; set; }

    public string? SupervisorLevel { get; set; }

    public string? FormFieldKey { get; set; }
}

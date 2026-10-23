using LINGYUN.Abp.ElsaNext.ApprovalFlow.Participants;
using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Workflows;

public sealed class ApprovalDesignBranch
{
    public string Id { get; set; } = "";

    public string Name { get; set; } = "";

    public string? Expression { get; set; }

    public List<ApprovalDesignNode> Nodes { get; set; } = [];
}

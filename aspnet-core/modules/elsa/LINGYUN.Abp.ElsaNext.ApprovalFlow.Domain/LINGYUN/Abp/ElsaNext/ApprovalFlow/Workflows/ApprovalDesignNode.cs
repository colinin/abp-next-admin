using LINGYUN.Abp.ElsaNext.ApprovalFlow.Participants;
using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Workflows;

public sealed class ApprovalDesignNode
{
    public string Id { get; set; } = "";

    public string Kind { get; set; } = "";

    public string Name { get; set; } = "";

    public string? SignMode { get; set; }

    public List<ApprovalDesignRule> Rules { get; set; } = [];

    public string? OnEmpty { get; set; }

    public List<ApprovalDesignAction> Actions { get; set; } = [];

    public string? ConditionExpression { get; set; }

    public List<ApprovalDesignBranch> Branches { get; set; } = [];

    public ApprovalDesignCondition? Condition { get; set; }
}

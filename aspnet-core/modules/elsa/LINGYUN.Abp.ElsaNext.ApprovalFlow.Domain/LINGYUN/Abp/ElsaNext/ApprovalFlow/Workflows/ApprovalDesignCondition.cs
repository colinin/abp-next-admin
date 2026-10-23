using LINGYUN.Abp.ElsaNext.ApprovalFlow.Participants;
using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Workflows;

public sealed class ApprovalDesignCondition
{
    public string? FieldKey { get; set; }

    public string? Operator { get; set; }

    public string? CompareValue { get; set; }

    public string? Expression { get; set; }
}

using LINGYUN.Abp.ElsaNext.ApprovalFlow.Participants;
using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Workflows;

public sealed class ApprovalDesignAction
{
    public string Key { get; set; } = "";

    public string Label { get; set; } = "";

    public string? Effect { get; set; }
}

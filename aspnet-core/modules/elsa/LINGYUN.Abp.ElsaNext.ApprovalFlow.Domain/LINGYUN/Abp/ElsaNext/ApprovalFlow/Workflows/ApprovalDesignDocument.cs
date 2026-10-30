using LINGYUN.Abp.ElsaNext.ApprovalFlow.Participants;
using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Workflows;

public sealed class ApprovalDesignDocument
{
    public int Version { get; set; }

    public Guid? FormDefinitionId { get; set; }

    public List<ApprovalDesignNode> Nodes { get; set; } = [];
}

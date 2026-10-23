using System;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Instances.Dtos;

public class InstanceDecisionResultDto
{
    public Guid? RecordId { get; set; }

    public bool Ignored { get; set; }
}

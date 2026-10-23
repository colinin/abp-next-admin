using System;
using System.Collections.Generic;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Records.Dtos;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Tasks.Dtos;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Instances.Dtos;

public class InstanceDetailDto
{
    public InstanceDto Instance { get; set; } = new();

    public List<RecordDto> Records { get; set; } = [];

    public List<NodeTaskDto> NodeTasks { get; set; } = [];

    public string? FlowDefinitionJson { get; set; }

    public Guid? FormDefinitionId { get; set; }
}

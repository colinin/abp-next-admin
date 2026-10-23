using System;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Instances.Dtos;

public class InstanceDto
{
    public Guid Id { get; set; }
    public Guid TemplateId { get; set; }
    public string TemplateName { get; set; } = "";
    public string? TemplateCategory { get; set; }
    public string? BusinessKey { get; set; }
    public string Title { get; set; } = "";
    public string Status { get; set; } = "";
    public string? CurrentNodeId { get; set; }
    public string? CurrentNodeName { get; set; }
    public string? InitiatorId { get; set; }
    public string? InitiatorDisplayName { get; set; }
    public string? ElsaWorkflowInstanceId { get; set; }
    public string? FormDataJson { get; set; }
    public string? Outcome { get; set; }
    public DateTimeOffset StartedAt { get; set; }
    public DateTimeOffset? FinishedAt { get; set; }
}

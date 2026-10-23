using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Tasks.Dtos;

public class TaskDto
{
    public string TaskId { get; set; } = "";
    public Guid InstanceId { get; set; }
    public string InstanceTitle { get; set; } = "";
    public string? TemplateName { get; set; }
    public string? NodeId { get; set; }
    public string? NodeName { get; set; }
    public string Title { get; set; } = "";
    public string? Summary { get; set; }
    public int Priority { get; set; }
    public DateTimeOffset? DueAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? FirstViewedAt { get; set; }
    public DateTimeOffset? CompletedAt { get; set; }
    public int Revision { get; set; }
    public IReadOnlyCollection<string> AllowedActions { get; set; } = [];
}

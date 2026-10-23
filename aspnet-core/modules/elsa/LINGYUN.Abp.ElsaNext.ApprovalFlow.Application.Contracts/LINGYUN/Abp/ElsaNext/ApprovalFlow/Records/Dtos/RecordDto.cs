using System;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Records.Dtos;

public class RecordDto
{
    public Guid Id { get; set; }
    public Guid InstanceId { get; set; }
    public string? NodeId { get; set; }
    public string? NodeName { get; set; }
    public string? TaskId { get; set; }
    public string ActionKey { get; set; } = "";
    public string? ActionLabel { get; set; }
    public string Effect { get; set; } = "";
    public string? Comment { get; set; }
    public string? ActorDisplayName { get; set; }
    public DateTimeOffset OccurredAt { get; set; }
    public DateTimeOffset? FirstViewedAt { get; set; }
    public DateTimeOffset? CompletedAt { get; set; }
}

using System;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.CarbonCopies.Dtos;

public class CarbonCopyDto
{
    public Guid Id { get; set; }
    public Guid? InstanceId { get; set; }
    public string? InstanceReference { get; set; }
    public string? NodeId { get; set; }
    public string Title { get; set; } = "";
    public string? Summary { get; set; }
    public string? SenderDisplayName { get; set; }
    public string? DataJson { get; set; }
    public DateTimeOffset NotifiedAt { get; set; }
    public bool IsRead { get; set; }
    public DateTimeOffset? ReadAt { get; set; }
}

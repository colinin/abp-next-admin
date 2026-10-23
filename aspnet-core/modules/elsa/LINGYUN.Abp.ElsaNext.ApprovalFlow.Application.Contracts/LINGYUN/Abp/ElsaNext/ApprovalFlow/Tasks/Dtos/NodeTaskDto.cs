namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Tasks.Dtos;

public class NodeTaskDto
{
    public string? NodeId { get; set; }

    public string? NodeName { get; set; }

    public string TaskId { get; set; } = "";

    public string Status { get; set; } = "";

    public string? ApproverDisplayName { get; set; }

    public string SignMode { get; set; } = "";
}

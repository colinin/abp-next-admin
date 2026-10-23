using System.ComponentModel.DataAnnotations;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Records;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Instances.Dtos;

public class InstanceDecisionDto
{
    [Required]
    public string TaskId { get; set; } = "";

    [Required]
    public string ActionKey { get; set; } = "";

    [DynamicStringLength(typeof(ApprovalRecordConsts), nameof(ApprovalRecordConsts.MaxCommentLength))]
    public string? Comment { get; set; }

    public string? DataJson { get; set; }

    public string? OperationId { get; set; }
}

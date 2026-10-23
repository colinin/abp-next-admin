using System.ComponentModel.DataAnnotations;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Nodes;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Records;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Instances.Dtos;

public class InstanceAddApprovalDto
{
    public string? TaskId { get; set; }

    [Required]
    public InstanceParticipantDto Approver { get; set; } = null!;

    [DynamicStringLength(typeof(ApprovalNodeTaskConsts), nameof(ApprovalNodeTaskConsts.MaxTitleLength))]
    public string? Title { get; set; }

    [DynamicStringLength(typeof(ApprovalRecordConsts), nameof(ApprovalRecordConsts.MaxCommentLength))]
    public string? Comment { get; set; }
}

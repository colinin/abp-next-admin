using System.ComponentModel.DataAnnotations;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Records;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Instances.Dtos;

public class InstanceTransferDto
{
    public string? TaskId { get; set; }

    [Required]
    public InstanceParticipantDto Transferee { get; set; } = null!;

    [DynamicStringLength(typeof(ApprovalRecordConsts), nameof(ApprovalRecordConsts.MaxCommentLength))]
    public string? Comment { get; set; }
}

using LINGYUN.Abp.ElsaNext.ApprovalFlow.Records;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Instances.Dtos;

public class InstanceReturnDto
{
    public string? TaskId { get; set; }

    [DynamicStringLength(typeof(ApprovalRecordConsts), nameof(ApprovalRecordConsts.MaxCommentLength))]
    public string? Comment { get; set; }
}

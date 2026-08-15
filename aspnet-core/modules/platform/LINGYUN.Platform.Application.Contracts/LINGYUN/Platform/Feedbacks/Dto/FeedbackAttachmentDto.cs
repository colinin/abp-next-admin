using System;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Platform.Feedbacks;
public class FeedbackAttachmentDto : CreationAuditedEntityDto<Guid>
{
    public string Name { get; set; } = default!;
    public string Url { get; set; } = default!;
    public long Size { get; set; }
}

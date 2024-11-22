using System;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Platform.Feedbacks;
public class FeedbackCommentDto : AuditedEntityDto<Guid>
{
    public string Content { get; set; }
}

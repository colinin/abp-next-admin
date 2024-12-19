using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Platform.Feedbacks;
public class FeedbackDto : ExtensibleAuditedEntityDto<Guid>
{
    public string Content { get; set; }
    public string Category { get; set; }
    public FeedbackStatus Status { get; set; }
    public List<FeedbackCommentDto> Comments { get; set; }
    public List<FeedbackAttachmentDto> Attachments { get; set; }
}

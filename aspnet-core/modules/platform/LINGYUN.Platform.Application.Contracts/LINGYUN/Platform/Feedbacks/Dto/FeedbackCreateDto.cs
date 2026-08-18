using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace LINGYUN.Platform.Feedbacks;
public class FeedbackCreateDto
{
    [Required]
    [DynamicStringLength(typeof(FeedbackConsts), nameof(FeedbackConsts.MaxContentLength))]
    public string Content { get; set; } = default!;

    [Required]
    [DynamicStringLength(typeof(FeedbackConsts), nameof(FeedbackConsts.MaxCategoryLength))]
    public string Category { get; set; } = default!;

    public List<FeedbackAttachmentTempFileCreateDto>? Attachments { get; set; }
}

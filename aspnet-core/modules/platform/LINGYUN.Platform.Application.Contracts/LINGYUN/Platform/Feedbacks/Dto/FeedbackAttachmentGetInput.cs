using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace LINGYUN.Platform.Feedbacks;
public class FeedbackAttachmentGetInput
{
    [Required]
    public Guid FeedbackId { get; set; }

    [Required]
    [DynamicStringLength(typeof(FeedbackAttachmentConsts), nameof(FeedbackAttachmentConsts.MaxNameLength))]
    public string Name { get; set; }
}

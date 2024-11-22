using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace LINGYUN.Platform.Feedbacks;
public abstract class FeedbackCommentCreateOrUpdateDto
{
    [Required]
    [DynamicStringLength(typeof(FeedbackCommentConsts), nameof(FeedbackCommentConsts.MaxContentLength))]
    public string Content { get; set; }
}

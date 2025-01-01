using Volo.Abp.Domain.Entities;

namespace LINGYUN.Platform.Feedbacks;
public class FeedbackCommentUpdateDto : FeedbackCommentCreateOrUpdateDto, IHasConcurrencyStamp
{
    public string ConcurrencyStamp { get; set; }
}

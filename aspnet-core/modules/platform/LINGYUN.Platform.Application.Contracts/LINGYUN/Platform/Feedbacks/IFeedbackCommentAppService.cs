using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace LINGYUN.Platform.Feedbacks;
public interface IFeedbackCommentAppService : IApplicationService
{
    Task<FeedbackCommentDto> ProgressAsync(FeedbackCommentCreateDto input);

    Task<FeedbackCommentDto> CloseAsync(FeedbackCommentCreateDto input);

    Task<FeedbackCommentDto> ResolveAsync(FeedbackCommentCreateDto input);

    Task<FeedbackCommentDto> UpdateAsync(Guid id, FeedbackCommentUpdateDto input);

    Task<FeedbackCommentDto> DeleteAsync(Guid id);
}

using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace LINGYUN.Platform.Feedbacks;
public interface IFeedbackAttachmentAppService : IApplicationService
{
    Task<FeedbackAttachmentTempFileDto> UploadAsync(FeedbackAttachmentUploadInput input);

    Task<IRemoteStreamContent> GetAsync(FeedbackAttachmentGetInput input);

    Task DeleteAsync(FeedbackAttachmentGetInput input);
}

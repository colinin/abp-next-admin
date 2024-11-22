using LINGYUN.Platform.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using Volo.Abp.Content;

namespace LINGYUN.Platform.Feedbacks;

[Authorize(PlatformPermissions.Feedback.Default)]
public class FeedbackAttachmentAppService : PlatformApplicationServiceBase, IFeedbackAttachmentAppService
{
    private readonly IFeedbackRepository _feedbackRepository;
    private readonly FeedbackAttachmentManager _feedbackAttachmentManager;

    public FeedbackAttachmentAppService(
        IFeedbackRepository feedbackRepository,
        FeedbackAttachmentManager feedbackAttachmentManager)
    {
        _feedbackRepository = feedbackRepository;
        _feedbackAttachmentManager = feedbackAttachmentManager;
    }

    public async virtual Task<FeedbackAttachmentTempFileDto> UploadAsync(FeedbackAttachmentUploadInput input)
    {
        var stream = input.File.GetStream();

        var tmpFile = await _feedbackAttachmentManager.SaveToTempAsync(stream);

        return new FeedbackAttachmentTempFileDto
        {
            Size = tmpFile.Size,
            Path = tmpFile.Path,
            Id = tmpFile.Id,
        };
    }

    public async virtual Task<IRemoteStreamContent> GetAsync(FeedbackAttachmentGetInput input)
    {
        var attachment = await GetFeedbackAttachmentAsync(input);

        var stream = await _feedbackAttachmentManager.DownloadAsync(attachment);

        return new RemoteStreamContent(stream, attachment.Name);
    }

    public async virtual Task DeleteAsync(FeedbackAttachmentGetInput input)
    {
        var feedback = await _feedbackRepository.GetAsync(input.FeedbackId);
        if (feedback.CreatorId != CurrentUser.Id)
        {
            await AuthorizationService.CheckAsync(PlatformPermissions.Feedback.ManageAttachments);
        }

        var attachment = feedback.FindAttachment(input.Name);

        feedback.RemoveAttachment(attachment.Name);

        await CurrentUnitOfWork.SaveChangesAsync();

        await _feedbackAttachmentManager.DeleteAsync(attachment);
    }

    protected async virtual Task<FeedbackAttachment> GetFeedbackAttachmentAsync(FeedbackAttachmentGetInput input)
    {
        var feedback = await _feedbackRepository.GetAsync(input.FeedbackId);
        var attachment = feedback.FindAttachment(input.Name);
        return attachment ?? throw new FeedbackAttachmentNotFoundException(input.Name);
    }
}

using LINGYUN.Platform.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Content;

namespace LINGYUN.Platform.Feedbacks;

[Area(PlatformRemoteServiceConsts.ModuleName)]
[RemoteService(Name = PlatformRemoteServiceConsts.RemoteServiceName)]
[Route($"api/{PlatformRemoteServiceConsts.ModuleName}/feedbacks")]
[Authorize(PlatformPermissions.Feedback.Default)]
public class FeedbackAttachmentController : AbpControllerBase, IFeedbackAttachmentAppService
{
    private readonly IFeedbackAttachmentAppService _service;
    public FeedbackAttachmentController(IFeedbackAttachmentAppService service)
    {
        _service = service;
    }

    [HttpGet]
    [Route("{FeedbackId}/attachments/{Name}")]
    public virtual Task<IRemoteStreamContent> GetAsync(FeedbackAttachmentGetInput input)
    {
        return _service.GetAsync(input);
    }

    [HttpPost]
    [Route("attachments/upload")]
    public virtual Task<FeedbackAttachmentTempFileDto> UploadAsync([FromForm] FeedbackAttachmentUploadInput input)
    {
        return _service.UploadAsync(input);
    }

    [HttpDelete]
    [Route("{FeedbackId}/attachments/{Name}")]
    public virtual Task DeleteAsync(FeedbackAttachmentGetInput input)
    {
        return _service.DeleteAsync(input);
    }
}

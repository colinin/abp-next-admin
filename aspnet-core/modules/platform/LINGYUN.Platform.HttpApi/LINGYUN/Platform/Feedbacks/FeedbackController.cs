using LINGYUN.Platform.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Platform.Feedbacks;

[Area(PlatformRemoteServiceConsts.ModuleName)]
[RemoteService(Name = PlatformRemoteServiceConsts.RemoteServiceName)]
[Route($"api/{PlatformRemoteServiceConsts.ModuleName}/feedbacks")]
[Authorize(PlatformPermissions.Feedback.Default)]
public class FeedbackController : AbpControllerBase, IFeedbackAppService
{
    private readonly IFeedbackAppService _service;
    public FeedbackController(IFeedbackAppService service)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize(PlatformPermissions.Feedback.Create)]
    public virtual Task<FeedbackDto> CreateAsync(FeedbackCreateDto input)
    {
        return _service.CreateAsync(input);
    }

    [HttpDelete]
    [Route("{id}")]
    [Authorize(PlatformPermissions.Feedback.Delete)]
    public virtual Task DeleteAsync(Guid id)
    {
        return _service.DeleteAsync(id);
    }

    [HttpGet]
    [Route("{id}")]
    public virtual Task<FeedbackDto> GetAsync(Guid id)
    {
        return _service.GetAsync(id);
    }

    [HttpGet]
    public virtual Task<PagedResultDto<FeedbackDto>> GetListAsync(FeedbackGetListInput input)
    {
        return _service.GetListAsync(input);
    }
}

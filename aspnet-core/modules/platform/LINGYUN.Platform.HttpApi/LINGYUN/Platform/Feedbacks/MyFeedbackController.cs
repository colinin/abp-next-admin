using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Platform.Feedbacks;

[Authorize]
[Area(PlatformRemoteServiceConsts.ModuleName)]
[RemoteService(Name = PlatformRemoteServiceConsts.RemoteServiceName)]
[Route($"api/{PlatformRemoteServiceConsts.ModuleName}/my-feedbacks")]
public class MyFeedbackController : AbpControllerBase, IMyFeedbackAppService
{
    private readonly IMyFeedbackAppService _service;
    public MyFeedbackController(IMyFeedbackAppService service)
    {
        _service = service;
    }

    [HttpGet]
    public virtual Task<PagedResultDto<FeedbackDto>> GetMyFeedbacksAsync(FeedbackGetListInput input)
    {
        return _service.GetMyFeedbacksAsync(input);
    }
}

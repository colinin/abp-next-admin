using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.Notifications;

[Authorize]
[Controller]
[RemoteService(Name = AbpNotificationsRemoteServiceConsts.RemoteServiceName)]
[Area(AbpNotificationsRemoteServiceConsts.ModuleName)]
[Route("api/notifications/my-notifilers")]
public class MyNotificationController : AbpControllerBase, IMyNotificationAppService
{
    protected IMyNotificationAppService MyNotificationAppService { get; }

    public MyNotificationController(
        IMyNotificationAppService myNotificationAppService)
    {
        MyNotificationAppService = myNotificationAppService;
    }

    [HttpPut]
    [Route("mark-read-state")]
    public async virtual Task MarkReadStateAsync(NotificationMarkReadStateInput input)
    {
        await MyNotificationAppService.MarkReadStateAsync(input);
    }

    [HttpDelete]
    [Route("{id}")]
    public async virtual Task DeleteAsync(long id)
    {
        await MyNotificationAppService.DeleteAsync(id);
    }


    [HttpGet]
    [Route("{id}")]
    public async virtual Task<UserNotificationDto> GetAsync(long id)
    {
        return await MyNotificationAppService.GetAsync(id);
    }

    [HttpGet]
    public async virtual Task<PagedResultDto<UserNotificationDto>> GetListAsync(UserNotificationGetByPagedDto input)
    {
        return await MyNotificationAppService.GetListAsync(input);
    }
}

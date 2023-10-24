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
[Route("api/notifications")]
public class NotificationController : AbpController, INotificationAppService
{
    protected INotificationAppService NotificationAppService { get; }

    public NotificationController(
        INotificationAppService notificationAppService)
    {
        NotificationAppService = notificationAppService;
    }

    [HttpPost]
    public async virtual Task SendAsync(NotificationSendDto input)
    {
        await NotificationAppService.SendAsync(input);
    }

    [HttpGet]
    [Route("assignables")]
    public async virtual Task<ListResultDto<NotificationGroupDto>> GetAssignableNotifiersAsync()
    {
        return await NotificationAppService.GetAssignableNotifiersAsync();
    }

    [HttpGet]
    [Route("assignable-templates")]
    public async virtual Task<ListResultDto<NotificationTemplateDto>> GetAssignableTemplatesAsync()
    {
        return await NotificationAppService.GetAssignableTemplatesAsync();
    }
}

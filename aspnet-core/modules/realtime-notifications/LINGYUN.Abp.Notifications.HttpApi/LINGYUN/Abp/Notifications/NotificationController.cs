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
public class NotificationController : AbpControllerBase, INotificationAppService
{
    protected INotificationAppService NotificationAppService { get; }

    public NotificationController(
        INotificationAppService notificationAppService)
    {
        NotificationAppService = notificationAppService;
    }

    [HttpPost]
    [Route("send")]
    public virtual Task SendAsync(NotificationSendDto input)
    {
        return NotificationAppService.SendAsync(input);
    }

    [HttpPost]
    [Route("send/template")]
    public virtual Task SendAsync(NotificationTemplateSendDto input)
    {
        return NotificationAppService.SendAsync(input);
    }

    [HttpGet]
    [Route("assignables")]
    public virtual Task<ListResultDto<NotificationGroupDto>> GetAssignableNotifiersAsync()
    {
        return NotificationAppService.GetAssignableNotifiersAsync();
    }

    [HttpGet]
    [Route("assignable-templates")]
    public virtual Task<ListResultDto<NotificationTemplateDto>> GetAssignableTemplatesAsync()
    {
        return NotificationAppService.GetAssignableTemplatesAsync();
    }
}

using LINGYUN.Abp.Notifications.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.Notifications.Definitions.Notifications;

[Controller]
[Authorize(NotificationsPermissions.Definition.Default)]
[RemoteService(Name = AbpNotificationsRemoteServiceConsts.RemoteServiceName)]
[Area(AbpNotificationsRemoteServiceConsts.ModuleName)]
[Route("api/notifications/definitions/notifications")]
public class NotificationDefinitionController : AbpControllerBase, INotificationDefinitionAppService
{
    private readonly INotificationDefinitionAppService _service;
    public NotificationDefinitionController(INotificationDefinitionAppService service)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize(NotificationsPermissions.Definition.Create)]
    public virtual Task<NotificationDefinitionDto> CreateAsync(NotificationDefinitionCreateDto input)
    {
        return _service.CreateAsync(input);
    }

    [HttpDelete]
    [Route("{name}")]
    [Authorize(NotificationsPermissions.Definition.Delete)]
    public virtual Task DeleteAsync(string name)
    {
        return _service.DeleteAsync(name);
    }

    [HttpGet]
    [Route("{name}")]
    public virtual Task<NotificationDefinitionDto> GetAsync(string name)
    {
        return _service.GetAsync(name);
    }

    [HttpGet]
    public virtual Task<ListResultDto<NotificationDefinitionDto>> GetListAsync(NotificationDefinitionGetListInput input)
    {
        return _service.GetListAsync(input);
    }

    [HttpPut]
    [Route("{name}")]
    [Authorize(NotificationsPermissions.Definition.Update)]
    public virtual Task<NotificationDefinitionDto> UpdateAsync(string name, NotificationDefinitionUpdateDto input)
    {
        return _service.UpdateAsync(name, input);
    }
}

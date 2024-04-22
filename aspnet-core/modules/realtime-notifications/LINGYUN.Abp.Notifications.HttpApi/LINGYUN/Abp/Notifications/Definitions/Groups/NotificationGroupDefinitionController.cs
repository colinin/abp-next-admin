using LINGYUN.Abp.Notifications.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.Notifications.Definitions.Groups;

[Controller]
[Authorize(NotificationsPermissions.GroupDefinition.Default)]
[RemoteService(Name = AbpNotificationsRemoteServiceConsts.RemoteServiceName)]
[Area(AbpNotificationsRemoteServiceConsts.ModuleName)]
[Route("api/notifications/definitions/groups")]
public class NotificationGroupDefinitionController : AbpControllerBase, INotificationGroupDefinitionAppService
{
    private readonly INotificationGroupDefinitionAppService _service;
    public NotificationGroupDefinitionController(INotificationGroupDefinitionAppService service)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize(NotificationsPermissions.GroupDefinition.Create)]
    public virtual Task<NotificationGroupDefinitionDto> CreateAsync(NotificationGroupDefinitionCreateDto input)
    {
        return _service.CreateAsync(input);
    }

    [HttpDelete]
    [Route("{name}")]
    [Authorize(NotificationsPermissions.GroupDefinition.Delete)]
    public virtual Task DeleteAsync(string name)
    {
        return _service.DeleteAsync(name);
    }

    [HttpGet]
    [Route("{name}")]
    public virtual Task<NotificationGroupDefinitionDto> GetAsync(string name)
    {
        return _service.GetAsync(name);
    }

    [HttpGet]
    public virtual Task<ListResultDto<NotificationGroupDefinitionDto>> GetListAsync(NotificationGroupDefinitionGetListInput input)
    {
        return _service.GetListAsync(input);
    }

    [HttpPut]
    [Route("{name}")]
    [Authorize(NotificationsPermissions.GroupDefinition.Update)]
    public virtual Task<NotificationGroupDefinitionDto> UpdateAsync(string name, NotificationGroupDefinitionUpdateDto input)
    {
        return _service.UpdateAsync(name, input);
    }
}

using LINGYUN.Abp.Notifications.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.Notifications;

[Controller]
[RemoteService(Name = AbpNotificationsRemoteServiceConsts.RemoteServiceName)]
[Area(AbpNotificationsRemoteServiceConsts.ModuleName)]
[Route("api/notifications/send-records")]
[Authorize(NotificationsPermissions.Notification.SendRecord.Default)]
public class NotificationSendRecordController : AbpControllerBase, INotificationSendRecordAppService
{
    private readonly INotificationSendRecordAppService _service;
    public NotificationSendRecordController(INotificationSendRecordAppService service)
    {
        _service = service;
    }

    [HttpDelete]
    [Route("{id}")]
    [Authorize(NotificationsPermissions.Notification.SendRecord.Delete)]
    public virtual Task DeleteAsync(long id)
    {
        return _service.DeleteAsync(id);
    }

    [HttpGet]
    public virtual Task<PagedResultDto<NotificationSendRecordDto>> GetListAsync(NotificationSendRecordGetPagedListInput input)
    {
        return _service.GetListAsync(input);
    }

    [HttpPost]
    [Route("{id}/re-send")]
    [Authorize(NotificationsPermissions.Notification.SendRecord.ReSend)]
    public virtual Task ReSendAsync(long id)
    {
        return _service.ReSendAsync(id);
    }
}

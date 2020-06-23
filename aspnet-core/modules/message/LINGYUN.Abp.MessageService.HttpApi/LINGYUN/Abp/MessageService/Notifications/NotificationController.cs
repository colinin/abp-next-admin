using LINGYUN.Abp.Notifications;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.MessageService.Notifications
{
    [RemoteService(Name = AbpMessageServiceConsts.RemoteServiceName)]
    [Route("api/notifications")]
    public class NotificationController : AbpController, INotificationAppService
    {
        private readonly INotificationAppService _notificationAppService;
        public NotificationController(
            INotificationAppService notificationAppService)
        {
            _notificationAppService = notificationAppService;
        }

        [HttpPut]
        [Route("ChangeReadState")]
        public virtual async Task ChangeUserNotificationReadStateAsync(UserNotificationChangeReadStateDto userNotificationChangeRead)
        {
            await _notificationAppService.ChangeUserNotificationReadStateAsync(userNotificationChangeRead);
        }

        [HttpDelete]
        [Route("{NotificationId}")]
        public virtual async Task DeleteAsync(NotificationGetByIdDto notificationGetById)
        {
            await _notificationAppService.DeleteAsync(notificationGetById);
        }

        [HttpDelete]
        [Route("User/{NotificationId}")]
        public virtual async Task DeleteUserNotificationAsync(NotificationGetByIdDto notificationGetById)
        {
            await _notificationAppService.DeleteUserNotificationAsync(notificationGetById);
        }
        
        [HttpGet]
        [Route("{NotificationId}")]
        public virtual async Task<NotificationInfo> GetAsync(NotificationGetByIdDto notificationGetById)
        {
            return await _notificationAppService.GetAsync(notificationGetById);
        }

        [HttpGet]
        [Route("User/{NotificationId}")]
        public virtual async Task<PagedResultDto<NotificationInfo>> GetUserNotificationsAsync(UserNotificationGetByPagedDto userNotificationGetByPaged)
        {
            return await _notificationAppService.GetUserNotificationsAsync(userNotificationGetByPaged);
        }
    }
}

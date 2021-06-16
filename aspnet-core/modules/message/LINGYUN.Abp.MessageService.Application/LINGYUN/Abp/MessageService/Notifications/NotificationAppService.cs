using LINGYUN.Abp.Notifications;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.MessageService.Notifications
{
    [Authorize]
    public class NotificationAppService : ApplicationService, INotificationAppService
    {
        protected INotificationSender NotificationSender { get; }

        public NotificationAppService(
            INotificationSender notificationSender)
        {
            NotificationSender = notificationSender;
        }

        public virtual async Task SendAsync(NotificationSendDto input)
        {
            UserIdentifier user = null;
            if (input.ToUserId.HasValue)
            {
                user = new UserIdentifier(input.ToUserId.Value, input.ToUserName);
            }
            await NotificationSender
                .SendNofiterAsync(
                    input.Name,
                    input.Data,
                    user,
                    CurrentTenant.Id,
                    input.Severity);
        }
    }
}

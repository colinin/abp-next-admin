using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.MessageService.Notifications
{
    [RemoteService(Name = AbpMessageServiceConsts.RemoteServiceName)]
    [Route("api/notifilers")]
    public class NotificationController : AbpController, INotificationAppService
    {
        protected INotificationAppService NotificationAppService { get; }

        public NotificationController(
            INotificationAppService notificationAppService)
        {
            NotificationAppService = notificationAppService;
        }

        [HttpPost]
        public virtual async Task SendAsync(NotificationSendDto input)
        {
            await NotificationAppService.SendAsync(input);
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.MessageService.Notifications
{
    [Authorize]
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
        public async virtual Task SendAsync(NotificationSendDto input)
        {
            await NotificationAppService.SendAsync(input);
        }

        [HttpGet]
        [Route("assignable-templates")]
        public async virtual Task<ListResultDto<NotificationTemplateDto>> GetAssignableTemplatesAsync()
        {
            return await NotificationAppService.GetAssignableTemplatesAsync();
        }
    }
}

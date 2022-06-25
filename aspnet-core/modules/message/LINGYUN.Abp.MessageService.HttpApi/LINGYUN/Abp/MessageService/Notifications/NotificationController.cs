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

        [HttpPost]
        [Route("templates")]
        public async virtual Task<NotificationTemplateDto> SetTemplateAsync(NotificationTemplateSetInput input)
        {
            return await NotificationAppService.SetTemplateAsync(input);
        }

        [HttpGet]
        [Route("templates/{Name}")]
        [Route("templates/{Culture}/{Name}")]
        public async virtual Task<NotificationTemplateDto> GetTemplateAsync(NotificationTemplateGetInput input)
        {
            return await NotificationAppService.GetTemplateAsync(input);
        }

        [HttpGet]
        [Route("templates")]
        public async virtual Task<ListResultDto<NotificationTemplateDto>> GetAssignableTemplatesAsync()
        {
            return await NotificationAppService.GetAssignableTemplatesAsync();
        }
    }
}

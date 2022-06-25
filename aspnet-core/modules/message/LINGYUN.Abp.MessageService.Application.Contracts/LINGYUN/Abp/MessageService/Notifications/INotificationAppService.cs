using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.MessageService.Notifications
{
    public interface INotificationAppService
    {
        Task<NotificationTemplateDto> SetTemplateAsync(NotificationTemplateSetInput input);

        Task<NotificationTemplateDto> GetTemplateAsync(NotificationTemplateGetInput input);

        Task<ListResultDto<NotificationTemplateDto>> GetAssignableTemplatesAsync();

        Task SendAsync(NotificationSendDto input);
    }
}

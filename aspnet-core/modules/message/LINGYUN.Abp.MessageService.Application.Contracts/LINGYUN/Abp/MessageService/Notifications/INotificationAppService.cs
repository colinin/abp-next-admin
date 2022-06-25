using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.MessageService.Notifications
{
    public interface INotificationAppService
    {
        Task<ListResultDto<NotificationTemplateDto>> GetAssignableTemplatesAsync();

        Task SendAsync(NotificationSendDto input);
    }
}

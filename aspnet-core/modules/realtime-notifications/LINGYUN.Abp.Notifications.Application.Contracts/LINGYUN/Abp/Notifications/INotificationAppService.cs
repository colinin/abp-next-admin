using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.Notifications;

public interface INotificationAppService
{
    Task<ListResultDto<NotificationGroupDto>> GetAssignableNotifiersAsync();

    Task<ListResultDto<NotificationTemplateDto>> GetAssignableTemplatesAsync();

    Task SendAsync(NotificationSendDto input);

    Task SendAsync(NotificationTemplateSendDto input);
}

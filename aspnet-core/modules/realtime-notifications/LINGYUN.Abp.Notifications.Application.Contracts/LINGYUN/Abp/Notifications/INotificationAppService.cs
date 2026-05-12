using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.Notifications;

public interface INotificationAppService
{
    Task<ListResultDto<NameValue<string>>> GetAssignableProvidersAsync();

    Task<ListResultDto<NotificationGroupDto>> GetAssignableNotifiersAsync();

    Task<ListResultDto<NotificationTemplateDto>> GetAssignableTemplatesAsync();

    Task SendAsync(NotificationSendDto input);

    Task SendTemplateAsync(NotificationTemplateSendDto input);
}

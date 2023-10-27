using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.Notifications.Definitions.Notifications;
public interface INotificationDefinitionAppService : IApplicationService
{
    Task<NotificationDefinitionDto> GetAsync(string name);
    Task DeleteAsync(string name);
    Task<NotificationDefinitionDto> CreateAsync(NotificationDefinitionCreateDto input);
    Task<NotificationDefinitionDto> UpdateAsync(string name, NotificationDefinitionUpdateDto input);
    Task<ListResultDto<NotificationDefinitionDto>> GetListAsync(NotificationDefinitionGetListInput input);
}

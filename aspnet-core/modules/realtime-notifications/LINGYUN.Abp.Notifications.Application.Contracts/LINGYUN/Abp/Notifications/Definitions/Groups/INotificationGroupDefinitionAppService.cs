using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.Notifications.Definitions.Groups;

public interface INotificationGroupDefinitionAppService : IApplicationService
{
    Task<NotificationGroupDefinitionDto> GetAsync(string name);

    Task DeleteAsync(string name);

    Task<ListResultDto<NotificationGroupDefinitionDto>> GetListAsync(NotificationGroupDefinitionGetListInput input);

    Task<NotificationGroupDefinitionDto> CreateAsync(NotificationGroupDefinitionCreateDto input);

    Task<NotificationGroupDefinitionDto> UpdateAsync(string name, NotificationGroupDefinitionUpdateDto input);
}

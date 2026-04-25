using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.Notifications;
public interface INotificationSendRecordAppService : IApplicationService
{
    Task DeleteAsync(long id);

    Task ReSendAsync(long id);

    Task<PagedResultDto<NotificationSendRecordDto>> GetListAsync(NotificationSendRecordGetPagedListInput input);
}

using LINGYUN.Abp.Notifications;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.MessageService.Notifications
{
    public interface IMyNotificationAppService : 
        
        IReadOnlyAppService<
            NotificationInfo,
            long,
            UserNotificationGetByPagedDto
            >,
        IDeleteAppService<long>
    {
        Task<ListResultDto<NotificationGroupDto>> GetAssignableNotifiersAsync();
    }
}

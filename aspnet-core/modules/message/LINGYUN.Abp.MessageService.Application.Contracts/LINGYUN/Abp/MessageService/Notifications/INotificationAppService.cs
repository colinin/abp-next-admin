using LINGYUN.Abp.Notifications;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.MessageService.Notifications
{
    public interface INotificationAppService : IApplicationService
    {
        /// <summary>
        /// 查询通知明细
        /// </summary>
        /// <param name="notificationGetById"></param>
        /// <returns></returns>
        Task<NotificationInfo> GetAsync(NotificationGetByIdDto notificationGetById);
        /// <summary>
        /// 删除通知
        /// </summary>
        /// <param name="notificationGetById"></param>
        /// <returns></returns>
        Task DeleteAsync(NotificationGetByIdDto notificationGetById);
        /// <summary>
        /// 删除用户通知
        /// </summary>
        /// <param name="notificationGetById"></param>
        /// <returns></returns>
        Task DeleteUserNotificationAsync(NotificationGetByIdDto notificationGetById);
        /// <summary>
        /// 变更通知阅读状态
        /// </summary>
        /// <param name="userNotificationChangeRead"></param>
        /// <returns></returns>
        Task ChangeUserNotificationReadStateAsync(UserNotificationChangeReadStateDto userNotificationChangeRead);
        /// <summary>
        /// 获取用户通知列表
        /// </summary>
        /// <param name="userNotificationGetByPaged"></param>
        /// <returns></returns>
        Task<PagedResultDto<NotificationInfo>> GetUserNotificationsAsync(UserNotificationGetByPagedDto userNotificationGetByPaged);
    }
}

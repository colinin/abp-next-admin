using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Abp.MessageService.Notifications
{
    public interface INotificationRepository : IBasicRepository<Notification, long>
    {
        Task<Notification> GetByIdAsync(
            long notificationId,
            CancellationToken cancellationToken = default);

        Task DeleteExpritionAsync(
            int batchCount,
            CancellationToken cancellationToken = default);
    }
}

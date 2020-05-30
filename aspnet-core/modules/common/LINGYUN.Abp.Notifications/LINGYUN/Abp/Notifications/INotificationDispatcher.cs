using System.Threading.Tasks;

namespace LINGYUN.Abp.Notifications
{
    public interface INotificationDispatcher
    {
        Task DispatcheAsync(NotificationInfo notification);
    }
}

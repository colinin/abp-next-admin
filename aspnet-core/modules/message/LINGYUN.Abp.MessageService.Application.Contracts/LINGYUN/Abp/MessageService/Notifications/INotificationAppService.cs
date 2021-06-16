using System.Threading.Tasks;

namespace LINGYUN.Abp.MessageService.Notifications
{
    public interface INotificationAppService
    {
        Task SendAsync(NotificationSendDto input);
    }
}

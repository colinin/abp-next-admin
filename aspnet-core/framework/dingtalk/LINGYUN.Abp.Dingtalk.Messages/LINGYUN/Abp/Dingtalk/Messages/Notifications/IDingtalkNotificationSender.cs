using LINGYUN.Abp.Dingtalk.Messages.Notifications.Models;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Dingtalk.Messages.Notifications;

public interface IDingtalkNotificationSender
{
    Task SendRobotNotifierAsync(RobotNotification notification);
}

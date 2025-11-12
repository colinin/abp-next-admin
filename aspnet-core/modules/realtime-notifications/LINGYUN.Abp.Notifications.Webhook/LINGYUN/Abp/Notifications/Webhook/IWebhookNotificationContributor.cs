using System.Threading.Tasks;

namespace LINGYUN.Abp.Notifications.Webhook;
public interface IWebhookNotificationContributor
{
    string Name { get; }
    Task ContributeAsync(IWebhookNotificationContext context);
}

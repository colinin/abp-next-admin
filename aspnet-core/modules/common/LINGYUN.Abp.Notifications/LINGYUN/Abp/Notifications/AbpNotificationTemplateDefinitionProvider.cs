using System.Linq;
using Volo.Abp.TextTemplating;

namespace LINGYUN.Abp.Notifications;
public class AbpNotificationTemplateDefinitionProvider : TemplateDefinitionProvider
{
    private readonly INotificationDefinitionManager _notificationDefinitionManager;

    public AbpNotificationTemplateDefinitionProvider(
        INotificationDefinitionManager notificationDefinitionManager)
    {
        _notificationDefinitionManager = notificationDefinitionManager;
    }

    public override void Define(ITemplateDefinitionContext context)
    {
        var notifications = _notificationDefinitionManager.GetAll().Where(n => n.Template != null);
        foreach (var notification in notifications)
        {
            context.Add(notification.Template);
        }
    }
}

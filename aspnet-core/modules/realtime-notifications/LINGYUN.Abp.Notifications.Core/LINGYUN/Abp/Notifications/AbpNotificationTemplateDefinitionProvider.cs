using System.Linq;
using Volo.Abp.TextTemplating;

namespace LINGYUN.Abp.Notifications;
public class AbpNotificationTemplateDefinitionProvider : TemplateDefinitionProvider
{
    private readonly IStaticNotificationDefinitionStore _staticNotificationDefinitionStore;

    public AbpNotificationTemplateDefinitionProvider(
        IStaticNotificationDefinitionStore staticNotificationDefinitionStore)
    {
        _staticNotificationDefinitionStore = staticNotificationDefinitionStore;
    }

    public override void Define(ITemplateDefinitionContext context)
    {
        var notifications = _staticNotificationDefinitionStore
            .GetNotificationsAsync().GetAwaiter().GetResult();

        foreach (var notification in notifications.Where(n => n.Template != null))
        {
            if (context.GetOrNull(notification.Template.Name) == null)
            {
                context.Add(notification.Template);
            }
        }
    }
}

using System;

namespace LINGYUN.Abp.Notifications.Templating;
public class NotificationTemplateResolveContext : INotificationTemplateResolveContext
{
    public IServiceProvider ServiceProvider { get; }

    public NotificationTemplate Template { get; }

    public object Model { get; set; }

    public bool Handled { get; set; }

    public bool HasResolvedModel()
    {
        return Handled || Model != null;
    }

    public NotificationTemplateResolveContext(
        NotificationTemplate template,
        IServiceProvider serviceProvider)
    {
        Template = template;
        ServiceProvider = serviceProvider;
    }
}

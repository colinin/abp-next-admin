using System.Collections.Generic;
using Volo.Abp.ObjectExtending;

namespace LINGYUN.Abp.Notifications.Definitions.Notifications;

public class NotificationDefinitionDto : ExtensibleObject
{
    public string Name { get; set; }

    public bool IsStatic { get; set; }

    public string GroupName { get; set; }

    public string DisplayName { get; set; }

    public string Description { get; set; }

    public bool AllowSubscriptionToClients { get; set; }

    public NotificationLifetime NotificationLifetime { get; set; }

    public NotificationType NotificationType { get; set; }

    public NotificationContentType ContentType { get; set; }

    public List<string> Providers { get; set; } = new List<string>();

    public string Template { get; set; }
}

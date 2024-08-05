using JetBrains.Annotations;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.Notifications;

public class NotificationGroupDefinition
{
    /// <summary>
    /// 通知组名称
    /// </summary>
    [NotNull]
    public string Name { get; set; }
    /// <summary>
    /// 通知组显示名称
    /// </summary>
    [NotNull]
    public ILocalizableString DisplayName
    {
        get => _displayName;
        set => _displayName = Check.NotNull(value, nameof(value));
    }
    private ILocalizableString _displayName;
    /// <summary>
    /// 通知组说明
    /// </summary>
    [CanBeNull]
    public ILocalizableString Description { get; set; }
    public bool AllowSubscriptionToClients { get; set; }
    public IReadOnlyList<NotificationDefinition> Notifications => _notifications.ToImmutableList();
    private readonly List<NotificationDefinition> _notifications;

    public Dictionary<string, object> Properties { get; }

    public object this[string name] {
        get => Properties.GetOrDefault(name);
        set => Properties[name] = value;
    }

    public static NotificationGroupDefinition Create(
        string name,
        ILocalizableString displayName = null,
        ILocalizableString description = null,
        bool allowSubscriptionToClients = false)
    {
        return new NotificationGroupDefinition(name, displayName, description, allowSubscriptionToClients);
    }

    protected internal NotificationGroupDefinition(
        string name,
        ILocalizableString displayName = null,
        ILocalizableString description = null,
        bool allowSubscriptionToClients = false)
    {
        Name = name;
        DisplayName = displayName ?? new FixedLocalizableString(Name);
        Description = description;
        AllowSubscriptionToClients = allowSubscriptionToClients;

        _notifications = new List<NotificationDefinition>();

        Properties = new Dictionary<string, object>();
    }

    public virtual NotificationDefinition AddNotification(
        string name,
       ILocalizableString displayName = null,
       ILocalizableString description = null,
       NotificationType notificationType = NotificationType.Application,
       NotificationLifetime lifetime = NotificationLifetime.Persistent,
       NotificationContentType contentType = NotificationContentType.Text,
       bool allowSubscriptionToClients = false)
    {
        var notification = new NotificationDefinition(
            name,
            displayName,
            description,
            notificationType,
            lifetime,
            contentType,
            allowSubscriptionToClients
        );

        _notifications.Add(notification);

        return notification;
    }

    public NotificationDefinition GetNotificationOrNull([NotNull] string name)
    {
        Check.NotNull(name, nameof(name));

        return _notifications.FirstOrDefault(x => x.Name == name);
    }
}

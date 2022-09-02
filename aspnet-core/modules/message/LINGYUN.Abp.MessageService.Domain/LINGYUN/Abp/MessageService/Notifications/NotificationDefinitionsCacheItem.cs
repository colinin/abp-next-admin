using LINGYUN.Abp.Notifications;
using System;
using System.Collections.Generic;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.MessageService.Notifications;

[Serializable]
[IgnoreMultiTenancy]
public class NotificationDefinitionsCacheItem
{
    private const string CacheKeyFormat = "n:Abp_Notifications;c:{0}";

    public NotificationDefinitionCacheItem[] Notifications { get; set; }

    public NotificationDefinitionsCacheItem()
    {
        Notifications = new NotificationDefinitionCacheItem[0];
    }

    public NotificationDefinitionsCacheItem(
        NotificationDefinitionCacheItem[] notifications)
    {
        Notifications = notifications;
    }

    public static string CalculateCacheKey(string culture)
    {
        return string.Format(CacheKeyFormat, culture);
    }
}

public class NotificationDefinitionCacheItem
{
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public string Description { get; set; }
    public NotificationLifetime Lifetime { get; set; }
    public NotificationType NotificationType { get; set; }
    public List<string> Providers { get; set; }
    public bool AllowSubscriptionToClients { get; set; }
    public Dictionary<string, object> Properties { get; set; }
    public NotificationDefinitionCacheItem()
    {
        Providers = new List<string>();
        Properties = new Dictionary<string, object>();
    }

    public NotificationDefinitionCacheItem(
        string name, 
        string displayName = null, 
        string description = null,
        NotificationLifetime lifetime = NotificationLifetime.Persistent,
        NotificationType notificationType = NotificationType.Application,
        List<string> providers = null,
        bool allowSubscriptionToClients  = false)
    {
        Name = name;
        DisplayName = displayName;
        Description = description;
        Lifetime = lifetime;
        NotificationType = notificationType;
        Providers = providers ?? new List<string>();
        AllowSubscriptionToClients = allowSubscriptionToClients;

        Properties = new Dictionary<string, object>();
    }
}

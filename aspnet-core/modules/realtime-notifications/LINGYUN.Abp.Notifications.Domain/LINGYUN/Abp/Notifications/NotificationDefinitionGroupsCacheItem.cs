using System;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.Notifications;

[Serializable]
[IgnoreMultiTenancy]
public class NotificationDefinitionGroupsCacheItem
{
    private const string CacheKeyFormat = "n:Abp_Notifications_Groups;c:{0}";

    public NotificationDefinitionGroupCacheItem[] Groups { get; set; }

    public NotificationDefinitionGroupsCacheItem()
    {
        Groups = new NotificationDefinitionGroupCacheItem[0];
    }

    public NotificationDefinitionGroupsCacheItem(
        NotificationDefinitionGroupCacheItem[] groups)
    {
        Groups = groups;
    }

    public static string CalculateCacheKey(string culture)
    {
        return string.Format(CacheKeyFormat, culture);
    }
}

public class NotificationDefinitionGroupCacheItem
{
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public string Description { get; set; }
    public bool AllowSubscriptionToClients { get; set; }
    public NotificationDefinitionGroupCacheItem()
    {

    }

    public NotificationDefinitionGroupCacheItem(
        string name, 
        string displayName = null, 
        string description = null,
        bool allowSubscriptionToClients  = false)
    {
        Name = name;
        DisplayName = displayName;
        Description = description;
        AllowSubscriptionToClients = allowSubscriptionToClients;
    }
}

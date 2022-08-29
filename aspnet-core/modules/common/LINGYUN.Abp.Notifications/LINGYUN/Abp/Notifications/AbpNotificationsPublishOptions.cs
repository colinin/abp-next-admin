using System;
using Volo.Abp.Collections;

namespace LINGYUN.Abp.Notifications;

public class AbpNotificationsPublishOptions
{
    /// <summary>
    /// 自定义通知发布提供者集合
    /// </summary>
    public ITypeList<INotificationPublishProvider> PublishProviders { get; }
    /// <summary>
    /// 可以自定义某个通知的格式
    /// </summary>
    public NotificationDataMappingDictionary NotificationDataMappings { get; }
    /// <summary>
    /// 过期时间
    /// 默认60天
    /// </summary>
    public TimeSpan ExpirationTime { get; set; }
    public AbpNotificationsPublishOptions()
    {
        PublishProviders = new TypeList<INotificationPublishProvider>();
        NotificationDataMappings = new NotificationDataMappingDictionary();

        ExpirationTime = TimeSpan.FromDays(60);
    }
}

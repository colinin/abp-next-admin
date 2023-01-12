using System;

namespace LINGYUN.Abp.WebhooksManagement;
public class WebhookManagementOptions
{
    /// <summary>
    /// Default: true.
    /// </summary>
    public bool SaveStaticWebhooksToDatabase { get; set; }
    /// <summary>
    /// Default: false.
    /// </summary>
    public bool IsDynamicWebhookStoreEnabled { get; set; }
    /// <summary>
    /// 缓存刷新时间
    /// default: 30 seconds
    /// </summary>
    public TimeSpan WebhooksCacheRefreshInterval { get; set; }
    /// <summary>
    /// 申请时间戳超时时间
    /// default: 2 minutes
    /// </summary>
    public TimeSpan WebhooksCacheStampTimeOut { get; set; }
    /// <summary>
    /// 时间戳过期时间
    /// default: 30 minutes
    /// </summary>
    public TimeSpan WebhooksCacheStampExpiration { get; set; }
    public WebhookManagementOptions()
    {
        IsDynamicWebhookStoreEnabled = true;
        SaveStaticWebhooksToDatabase = true;

        WebhooksCacheRefreshInterval = TimeSpan.FromSeconds(30);
        WebhooksCacheStampTimeOut = TimeSpan.FromMinutes(2);
        // 30分钟过期重新刷新缓存
        WebhooksCacheStampExpiration = TimeSpan.FromMinutes(30);
    }
}

using System;

namespace LINGYUN.Abp.Tencent.QQ;

public class AbpTencentQQCacheItem
{
    public const string CacheKeyFormat = "pn:{0},pk:{1}";
    public string AppId { get; set; }
    public string AppKey { get; set; }
    public bool IsMobile { get; set; }
    public AbpTencentQQCacheItem()
    {

    }

    public AbpTencentQQCacheItem(
        string appId,
        string appKey,
        bool isMobile = false)
    {
        AppId = appId;
        AppKey = appKey;
        IsMobile = isMobile;
    }

    public static string CalculateCacheKey(Guid? tenantId = null)
    {
        return string.Format(CacheKeyFormat, tenantId.HasValue ? tenantId.Value.ToString() : "global", "tenant-qq");
    }
}

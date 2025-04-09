using System;
using System.Collections.Generic;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.Gdpr;

[Serializable]
[IgnoreMultiTenancy]
public class GdprRequestCacheItem
{
    private const string CacheKeyFormat = "uid:{0};rid:{1}";

    public Guid UserId { get; set; }
    public Guid RequestId { get; set; }
    public List<GdprInfoCacheItem> Infos { get; set; }
    public GdprRequestCacheItem()
    {
        Infos = new List<GdprInfoCacheItem>();
    }

    public GdprRequestCacheItem(Guid userId, Guid requestId)
    {
        UserId = userId;
        RequestId = requestId;

        Infos = new List<GdprInfoCacheItem>();
    }

    public static string CalculateCacheKey(Guid userId, Guid requestId)
    {
        return string.Format(CacheKeyFormat, userId, requestId);
    }
}

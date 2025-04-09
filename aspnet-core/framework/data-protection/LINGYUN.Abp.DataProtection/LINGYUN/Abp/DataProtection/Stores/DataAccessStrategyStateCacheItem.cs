using System;
using Volo.Abp.Caching;

namespace LINGYUN.Abp.DataProtection.Stores;

[Serializable]
[CacheName("AbpDataProtectionStrategyStates")]
public class DataAccessStrategyStateCacheItem
{
    private const string CacheKeyFormat = "sn:{0};si:{1}";
    /// <summary>
    /// 权限主体
    /// </summary>
    public string SubjectName { get; set; }

    /// <summary>
    /// 权限主体标识
    /// </summary>
    public string SubjectId { get; set; }

    /// <summary>
    /// 权限策略
    /// </summary>
    public DataAccessStrategy Strategy { get; set; }
    public DataAccessStrategyStateCacheItem()
    {

    }

    public DataAccessStrategyStateCacheItem(
        string subjectName, 
        string subjectId, 
        DataAccessStrategy strategy)
    {
        SubjectName = subjectName;
        SubjectId = subjectId;
        Strategy = strategy;
    }

    public static string CalculateCacheKey(string subjectName, string subjectId)
    {
        return string.Format(CacheKeyFormat, subjectName, subjectId);
    }
}

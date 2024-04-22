using System;

namespace LINGYUN.Abp.LocalizationManagement;

public class AbpLocalizationManagementOptions
{
    /// <summary>
    /// 申请时间戳超时时间
    /// default: 2 minutes
    /// </summary>
    public TimeSpan LocalizationCacheStampTimeOut { get; set; }
    /// <summary>
    /// 时间戳过期时间
    /// default: 30 minutes
    /// </summary>
    public TimeSpan LocalizationCacheStampExpiration { get; set; }

    public AbpLocalizationManagementOptions()
    {
        LocalizationCacheStampTimeOut = TimeSpan.FromMinutes(2);
        // 30分钟过期重新刷新缓存
        LocalizationCacheStampExpiration = TimeSpan.FromMinutes(30);
    }
}

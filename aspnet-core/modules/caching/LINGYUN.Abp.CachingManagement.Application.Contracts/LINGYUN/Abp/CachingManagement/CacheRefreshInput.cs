using System;

namespace LINGYUN.Abp.CachingManagement;

public class CacheRefreshInput
{
    public string Key { get; set; }
    public DateTime? AbsoluteExpiration { get; set; }
    public DateTime? SlidingExpiration { get; set; }
}

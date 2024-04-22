using System;
using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.CachingManagement;

public class RefreshCacheRequest
{
    [Required]
    public string Key { get; }
    public TimeSpan? AbsoluteExpiration { get; }
    public TimeSpan? SlidingExpiration { get; }
    public RefreshCacheRequest(
        string key, 
        TimeSpan? absoluteExpiration = null, 
        TimeSpan? slidingExpiration = null)
    {
        Key = key;
        AbsoluteExpiration = absoluteExpiration;
        SlidingExpiration = slidingExpiration;
    }
}

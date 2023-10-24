using System;
using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.CachingManagement;
public class SetCacheRequest
{
    [Required]
    public string Key { get; }
    [Required]
    public string Value { get; }
    public TimeSpan? AbsoluteExpiration { get; }
    public TimeSpan? SlidingExpiration { get; }
    public SetCacheRequest(
        string key,
        string value,
        TimeSpan? absoluteExpiration = null,
        TimeSpan? slidingExpiration = null)
    {
        Key = key;
        Value = value;
        AbsoluteExpiration = absoluteExpiration;
        SlidingExpiration = slidingExpiration;
    }
}

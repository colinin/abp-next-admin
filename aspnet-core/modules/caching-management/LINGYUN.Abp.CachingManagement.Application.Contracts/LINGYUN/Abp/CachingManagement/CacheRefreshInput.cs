using System;
using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.CachingManagement;

public class CacheRefreshInput
{
    [Required]
    public string Key { get; set; }
    public DateTime? AbsoluteExpiration { get; set; }
    public DateTime? SlidingExpiration { get; set; }
}

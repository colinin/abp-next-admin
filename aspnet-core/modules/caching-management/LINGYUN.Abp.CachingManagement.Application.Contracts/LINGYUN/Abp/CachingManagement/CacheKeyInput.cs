using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.CachingManagement;

public class CacheKeyInput
{
    [Required]
    public string Key { get; set; }
}

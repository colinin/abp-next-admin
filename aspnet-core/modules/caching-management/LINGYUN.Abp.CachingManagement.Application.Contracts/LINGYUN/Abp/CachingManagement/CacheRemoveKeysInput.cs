using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.CachingManagement;

public class CacheRemoveKeysInput
{
    [Required]
    public string[] Keys { get; set; }
}

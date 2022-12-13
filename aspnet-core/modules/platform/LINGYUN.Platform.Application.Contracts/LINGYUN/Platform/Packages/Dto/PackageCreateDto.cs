using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace LINGYUN.Platform.Packages;
public class PackageCreateDto : PackageCreateOrUpdateDto
{
    /// <summary>
    /// 名称
    /// </summary>
    [Required]
    [DynamicMaxLength(typeof(PackageConsts), nameof(PackageConsts.MaxNameLength))]
    public string Name { get; set; }
    /// <summary>
    /// 版本
    /// </summary>
    [Required]
    [DynamicMaxLength(typeof(PackageConsts), nameof(PackageConsts.MaxVersionLength))] 
    public string Version { get; set; }
}

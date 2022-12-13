using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace LINGYUN.Platform.Packages;

public abstract class PackageCreateOrUpdateDto
{
    /// <summary>
    /// 版本说明
    /// </summary>
    [Required]
    [DynamicMaxLength(typeof(PackageConsts), nameof(PackageConsts.MaxNoteLength))]
    public string Note { get; set; }
    /// <summary>
    /// 描述
    /// </summary>
    [DynamicMaxLength(typeof(PackageConsts), nameof(PackageConsts.MaxDescriptionLength))] 
    public string Description { get; set; }
    /// <summary>
    /// 强制更新
    /// </summary>
    public bool ForceUpdate { get; set; }

    [DynamicMaxLength(typeof(PackageConsts), nameof(PackageConsts.MaxAuthorsLength))]
    public string Authors { get; set; }

    public PackageLevel Level { get; set; }
}

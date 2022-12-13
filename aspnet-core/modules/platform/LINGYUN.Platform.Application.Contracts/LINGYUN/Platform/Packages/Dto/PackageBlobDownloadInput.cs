using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace LINGYUN.Platform.Packages;

public class PackageBlobDownloadInput
{
    [Required]
    [DynamicMaxLength(typeof(PackageBlobConsts), nameof(PackageBlobConsts.MaxNameLength))]
    public string Name { get; set; }
}

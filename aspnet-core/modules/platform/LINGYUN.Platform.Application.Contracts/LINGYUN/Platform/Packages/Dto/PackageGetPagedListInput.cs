using Volo.Abp.Application.Dtos;
using Volo.Abp.Validation;

namespace LINGYUN.Platform.Packages;

public class PackageGetPagedListInput : PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }

    [DynamicMaxLength(typeof(PackageConsts), nameof(PackageConsts.MaxNameLength))]
    public string Name { get; set; }

    [DynamicMaxLength(typeof(PackageConsts), nameof(PackageConsts.MaxNoteLength))]
    public string Note { get; set; }

    [DynamicMaxLength(typeof(PackageConsts), nameof(PackageConsts.MaxVersionLength))]
    public string Version { get; set; }

    [DynamicMaxLength(typeof(PackageConsts), nameof(PackageConsts.MaxDescriptionLength))]
    public string Description { get; set; }

    public bool? ForceUpdate { get; set; }

    [DynamicMaxLength(typeof(PackageConsts), nameof(PackageConsts.MaxAuthorsLength))]
    public string Authors { get; set; }
}

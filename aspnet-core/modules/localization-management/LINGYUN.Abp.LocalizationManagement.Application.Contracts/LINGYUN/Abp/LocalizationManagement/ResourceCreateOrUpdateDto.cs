using Volo.Abp.Validation;

namespace LINGYUN.Abp.LocalizationManagement;

public abstract class ResourceCreateOrUpdateDto
{
    public bool Enable { get; set; } = true;

    [DynamicStringLength(typeof(ResourceConsts), nameof(ResourceConsts.MaxNameLength))]
    public string DisplayName { get; set; }

    [DynamicStringLength(typeof(ResourceConsts), nameof(ResourceConsts.MaxNameLength))]
    public string Description { get; set; }

    [DynamicStringLength(typeof(ResourceConsts), nameof(ResourceConsts.MaxNameLength))]
    public string DefaultCultureName { get; set; }
}

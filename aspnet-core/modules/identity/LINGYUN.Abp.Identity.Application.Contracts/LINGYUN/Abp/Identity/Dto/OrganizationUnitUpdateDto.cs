using Volo.Abp.Identity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.Identity;

public class OrganizationUnitUpdateDto : ExtensibleObject
{
    [DynamicStringLength(typeof(OrganizationUnitConsts), nameof(OrganizationUnitConsts.MaxDisplayNameLength))]
    public string? DisplayName { get; set; }
}

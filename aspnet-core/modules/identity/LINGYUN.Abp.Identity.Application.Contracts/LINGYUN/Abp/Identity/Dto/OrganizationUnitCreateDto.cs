using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Identity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.Identity;

public class OrganizationUnitCreateDto : ExtensibleObject
{
    [Required]
    [DynamicStringLength(typeof(OrganizationUnitConsts), nameof(OrganizationUnitConsts.MaxDisplayNameLength))]
    public string DisplayName { get; set; } = default!;

    public Guid? ParentId { get; set; }
}

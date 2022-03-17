using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.Saas.Tenants;

public abstract class TenantCreateOrUpdateBase : ExtensibleObject
{
    [Required]
    [DynamicStringLength(typeof(TenantConsts), nameof(TenantConsts.MaxNameLength))]

    public string Name { get; set; }

    public bool IsActive { get; set; } = true;

    public Guid? EditionId { get; set; }

    public DateTime? EnableTime { get; set; }

    public DateTime? DisableTime { get; set; }
}
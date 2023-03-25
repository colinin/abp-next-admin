using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Data;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.PermissionManagement.Definitions;

public abstract class PermissionDefinitionCreateOrUpdateDto : IHasExtraProperties
{
    [Required]
    [DynamicStringLength(typeof(PermissionDefinitionRecordConsts), nameof(PermissionDefinitionRecordConsts.MaxDisplayNameLength))]
    public string DisplayName { get; set; }

    [DynamicStringLength(typeof(PermissionDefinitionRecordConsts), nameof(PermissionDefinitionRecordConsts.MaxNameLength))]
    public string ParentName { get; set; }

    public bool IsEnabled { get; set; }

    public MultiTenancySides? MultiTenancySide { get; set; }

    public List<string> Providers { get; set; } = new List<string>();

    public List<string> StateCheckers { get; set; } = new List<string>();

    public ExtraPropertyDictionary ExtraProperties { get; set; } = new ExtraPropertyDictionary();
}

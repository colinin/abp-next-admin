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

    public MultiTenancySides MultiTenancySide { get; set; } = MultiTenancySides.Both;

    public List<string> Providers { get; set; } = new List<string>();

    [DynamicStringLength(typeof(PermissionDefinitionRecordConsts), nameof(PermissionDefinitionRecordConsts.MaxStateCheckersLength))]
    public string StateCheckers { get; set; }

    public ExtraPropertyDictionary ExtraProperties { get; set; } = new ExtraPropertyDictionary();
}

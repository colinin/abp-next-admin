using System.ComponentModel.DataAnnotations;
using Volo.Abp.Data;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.PermissionManagement.Definitions;

public abstract class PermissionGroupDefinitionCreateOrUpdateDto : IHasExtraProperties
{
    [Required]
    [DynamicStringLength(typeof(PermissionGroupDefinitionRecordConsts), nameof(PermissionGroupDefinitionRecordConsts.MaxDisplayNameLength))]
    public string DisplayName { get; set; }

    public ExtraPropertyDictionary ExtraProperties { get; set; } = new ExtraPropertyDictionary();
}

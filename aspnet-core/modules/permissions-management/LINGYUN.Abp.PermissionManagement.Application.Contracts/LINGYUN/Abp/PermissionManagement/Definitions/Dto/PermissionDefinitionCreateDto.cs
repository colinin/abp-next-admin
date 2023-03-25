using System.ComponentModel.DataAnnotations;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.PermissionManagement.Definitions;

public class PermissionDefinitionCreateDto : PermissionDefinitionCreateOrUpdateDto
{
    [Required]
    [DynamicStringLength(typeof(PermissionDefinitionRecordConsts), nameof(PermissionDefinitionRecordConsts.MaxNameLength))]
    public string Name { get; set; }

    [Required]
    [DynamicStringLength(typeof(PermissionGroupDefinitionRecordConsts), nameof(PermissionGroupDefinitionRecordConsts.MaxNameLength))]
    public string GroupName { get; set; }
}

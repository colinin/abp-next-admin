using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.DataProtectionManagement;
public class OrganizationUnitEntityRuleCreateDto : EntityRuleCreateOrUpdateDto
{
    [Required]
    public Guid EntityTypeId { get; set; }

    [Required]
    public Guid OrgId { get; set; }

    [Required]
    [DynamicStringLength(typeof(OrganizationUnitEntityRuleConsts), nameof(OrganizationUnitEntityRuleConsts.MaxCodeLength))]
    public string OrgCode { get; set; }
}

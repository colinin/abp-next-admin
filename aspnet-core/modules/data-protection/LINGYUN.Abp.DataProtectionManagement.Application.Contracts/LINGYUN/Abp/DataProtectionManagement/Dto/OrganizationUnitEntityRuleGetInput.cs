using LINGYUN.Abp.DataProtection;
using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.DataProtectionManagement;

public class OrganizationUnitEntityRuleGetInput
{
    [Required]
    [DynamicStringLength(typeof(OrganizationUnitEntityRuleConsts), nameof(OrganizationUnitEntityRuleConsts.MaxCodeLength))]
    public string OrgCode { get; set; }

    [Required]
    public Guid EntityTypeId { get; set; }

    [Required]
    public DataAccessOperation Operation { get; set; }
}

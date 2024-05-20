using System;

namespace LINGYUN.Abp.DataProtectionManagement;
public class OrganizationUnitEntityRuleDto : EntityRuleDtoBase
{
    public Guid OrgId { get; set; }
    public string OrgCode { get; set; }
}

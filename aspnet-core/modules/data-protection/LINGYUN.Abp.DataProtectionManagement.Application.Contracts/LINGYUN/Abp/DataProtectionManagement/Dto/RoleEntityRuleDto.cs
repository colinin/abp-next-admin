using System;

namespace LINGYUN.Abp.DataProtectionManagement;

public class RoleEntityRuleDto : EntityRuleDtoBase
{
    public Guid RoleId { get; set; }
    public string RoleName { get; set; }
}

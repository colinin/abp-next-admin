using LINGYUN.Abp.DataProtection;
using System;
using Volo.Abp;

namespace LINGYUN.Abp.DataProtectionManagement;
public class RoleEntityRule : EntityRuleBase
{
    public virtual Guid RoleId { get; protected set; }
    public virtual string RoleName { get; protected set; }
    protected RoleEntityRule()
    {
    }

    public RoleEntityRule(
        Guid id, 
        Guid roleId,
        string roleName,
        Guid entityTypeId,
        string entityTypeFullName,
        DataAccessOperation operation,
        string allowProperties = null,
        DataAccessFilterGroup filterGroup = null, 
        Guid? tenantId = null)
        : base(id, entityTypeId, entityTypeFullName, operation, allowProperties, filterGroup, tenantId)
    {
        RoleId = roleId;
        RoleName = Check.NotNullOrWhiteSpace(roleName, nameof(roleName), RoleEntityRuleConsts.MaxRuletNameLength);
    }
}

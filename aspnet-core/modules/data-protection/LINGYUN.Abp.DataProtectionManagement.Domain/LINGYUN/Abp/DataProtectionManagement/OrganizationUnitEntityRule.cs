using LINGYUN.Abp.DataProtection;
using System;
using Volo.Abp;

namespace LINGYUN.Abp.DataProtectionManagement;
public class OrganizationUnitEntityRule : EntityRuleBase
{
    public virtual Guid OrgId { get; protected set; }
    public virtual string OrgCode { get; protected set; }

    protected OrganizationUnitEntityRule()
    {
    }

    public OrganizationUnitEntityRule(
        Guid id,
        Guid orgId,
        string orgCode,
        Guid entityTypeId,
        string entityTypeFullName,
        DataAccessOperation operation,
        string allowProperties = null,
        DataAccessFilterGroup filterGroup = null,
        Guid? tenantId = null)
        : base(id, entityTypeId, entityTypeFullName, operation, allowProperties, filterGroup, tenantId)
    {
        OrgId = orgId;
        OrgCode = Check.NotNullOrWhiteSpace(orgCode, nameof(orgCode), OrganizationUnitEntityRuleConsts.MaxCodeLength);
    }
}

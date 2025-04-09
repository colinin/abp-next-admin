using System;

namespace LINGYUN.Abp.DataProtection;

public class FakeProtectionObjectAuth : DataAuthBase<FakeProtectionObject, int>
{
    public FakeProtectionObjectAuth()
    {
    }

    public FakeProtectionObjectAuth(
        int entityId, 
        string role, 
        string organizationUnit, 
        Guid? tenantId = null) 
        : base(entityId, role, organizationUnit, tenantId)
    {
    }
}

using System;
using System.Collections.Generic;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.OssManagement;
public abstract class GetFileMultiTenancyInput
{
    /// <summary>
    /// 解决通过路由传递租户标识时,abp写入cookies
    /// </summary>
    public string TenantId { get; set; }

    public virtual Guid? GetTenantId(ICurrentTenant currentTenant)
    {
        if (!TenantId.IsNullOrWhiteSpace())
        {
            if ("g".Equals(TenantId, StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }
            if ("global".Equals(TenantId, StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }
            if (Guid.TryParse(TenantId, out var tenantId))
            {
                return tenantId;
            }
        }
        return currentTenant.Id;
    }
}

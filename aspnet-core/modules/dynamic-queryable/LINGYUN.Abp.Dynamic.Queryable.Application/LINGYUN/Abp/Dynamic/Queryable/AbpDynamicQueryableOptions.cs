using System.Collections.Generic;
using Volo.Abp.Auditing;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.Dynamic.Queryable;

public class AbpDynamicQueryableOptions
{
    public List<string> IgnoreFields { get; }

    public AbpDynamicQueryableOptions()
    {
        IgnoreFields = new List<string>
        {
            nameof(IMultiTenant.TenantId),
            nameof(IDeletionAuditedObject.IsDeleted),
            nameof(IDeletionAuditedObject.DeleterId),
            nameof(IDeletionAuditedObject.DeletionTime),
        };
    }
}

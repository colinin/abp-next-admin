using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.RulesEngineManagement;
public class ParamRecord : Entity<Guid>, IMultiTenant
{
    public virtual Guid? TenantId { get; protected set; }
    public virtual string Name { get; protected set; }
    public virtual string Expression { get; protected set; }
    protected ParamRecord()
    {

    }

    public ParamRecord(Guid id, string name, string expression, Guid? tenantId = null)
        : base(id)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), ParamRecordConsts.MaxNameLength);
        Expression = Check.NotNullOrWhiteSpace(expression, nameof(expression), ParamRecordConsts.MaxExpressionLength);
        TenantId = tenantId;
    }
}

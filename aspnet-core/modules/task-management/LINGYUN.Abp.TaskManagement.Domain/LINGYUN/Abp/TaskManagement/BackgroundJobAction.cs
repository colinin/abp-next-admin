using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.TaskManagement;

public class BackgroundJobAction : AuditedAggregateRoot<Guid>, IMultiTenant
{
    public virtual Guid? TenantId { get; protected set; }
    /// <summary>
    /// 作业标识
    /// </summary>
    public virtual string JobId { get; protected set; }
    /// <summary>
    /// 名称
    /// </summary>
    public virtual string Name { get; protected set; }
    /// <summary>
    /// 是否启用
    /// </summary>
    public virtual bool IsEnabled { get; set; }
    /// <summary>
    /// 参数
    /// </summary>
    public virtual ExtraPropertyDictionary Paramters { get; set; }

    protected BackgroundJobAction() { }

    public BackgroundJobAction(
        Guid id,
        string jobId,
        string name,
        IDictionary<string, object> paramters,
        Guid? tenantId = null) : base(id)
    {
        JobId = Check.NotNullOrWhiteSpace(jobId, nameof(jobId), BackgroundJobActionConsts.MaxJobIdLength);
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), BackgroundJobActionConsts.MaxNameLength);
        TenantId = tenantId;

        IsEnabled = true;

        Paramters = new ExtraPropertyDictionary();
        Paramters.AddIfNotContains(paramters);
    }
}

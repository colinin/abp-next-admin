using System;
using System.Collections.Generic;
using Volo.Abp.Auditing;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.DataProtection;
public class AbpDataProtectionOptions
{
    /// <summary>
    /// 是否启用数据审计
    /// 默认: true
    /// </summary>
    public bool IsEnabled { get; set; }
    /// <summary>
    /// 权限主体
    /// </summary>
    public IList<IDataAccessSubjectContributor> SubjectContributors { get; set; }
    /// <summary>
    /// 过滤字段关键字
    /// </summary>
    public IDictionary<string, IDataAccessKeywordContributor> KeywordContributors { get; set; }
    /// <summary>
    /// 数据操作
    /// </summary>
    public IDictionary<DataAccessFilterOperate, IDataAccessOperateContributor> OperateContributors { get; set; }
    /// <summary>
    /// 忽略审计字段列表
    /// </summary>
    public IList<string> IgnoreAuditedProperties { get; set; }
    public AbpDataProtectionOptions()
    {
        IsEnabled = true;
        SubjectContributors = new List<IDataAccessSubjectContributor>();
        KeywordContributors = new Dictionary<string, IDataAccessKeywordContributor>();
        OperateContributors = new Dictionary<DataAccessFilterOperate, IDataAccessOperateContributor>();

        IgnoreAuditedProperties = new List<string>
        {
            nameof(IEntity<Guid>.Id),
            nameof(IAuditedObject.LastModifierId),
            nameof(IAuditedObject.LastModificationTime),
            nameof(IAuditedObject.CreatorId),
            nameof(IAuditedObject.CreationTime),
            nameof(IDeletionAuditedObject.IsDeleted),
            nameof(IDeletionAuditedObject.DeleterId),
            nameof(IDeletionAuditedObject.DeletionTime),
            nameof(IMultiTenant.TenantId),
            nameof(IHasEntityVersion.EntityVersion),
            nameof(IHasConcurrencyStamp.ConcurrencyStamp),
            nameof(IHasExtraProperties.ExtraProperties),
        };
    }
}

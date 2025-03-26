using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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
    public IList<IDataAccessSubjectContributor> SubjectContributors { get; }
    /// <summary>
    /// 过滤字段关键字
    /// </summary>
    public IDictionary<string, IDataAccessKeywordContributor> KeywordContributors { get; }
    /// <summary>
    /// 数据操作
    /// </summary>
    public IDictionary<DataAccessFilterOperate, IDataAccessOperateContributor> OperateContributors { get; }
    /// <summary>
    /// 默认实体过滤
    /// </summary>
    public IDictionary<Type, Func<IServiceProvider, Type, DataAccessOperation, LambdaExpression>> DefaultEntityFilters { get; }
    /// <summary>
    /// 实体忽略字段
    /// </summary>
    public IDictionary<Type, string[]> EntityIgnoreProperties { get; }
    /// <summary>
    /// 全局忽略字段列表(不参与字段级权限校验)
    /// </summary>
    public IList<string> GlobalIgnoreProperties { get; set; }
    /// <summary>
    /// 审计字段列表(这些字段写入受保护实体的属性列表中,用于前端过滤)
    /// </summary>
    public IList<string> AuditedObjectProperties { get; set; }
    public AbpDataProtectionOptions()
    {
        IsEnabled = true;
        SubjectContributors = new List<IDataAccessSubjectContributor>();
        KeywordContributors = new Dictionary<string, IDataAccessKeywordContributor>();
        OperateContributors = new Dictionary<DataAccessFilterOperate, IDataAccessOperateContributor>();
        DefaultEntityFilters = new Dictionary<Type, Func<IServiceProvider, Type, DataAccessOperation, LambdaExpression>>();
        EntityIgnoreProperties = new Dictionary<Type, string[]>();

        GlobalIgnoreProperties = new List<string>
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

        AuditedObjectProperties = new List<string>
        {
            nameof(IAuditedObject.LastModifierId),
            nameof(IAuditedObject.LastModificationTime),
            nameof(IAuditedObject.CreatorId),
            nameof(IAuditedObject.CreationTime),
        };
    }
}

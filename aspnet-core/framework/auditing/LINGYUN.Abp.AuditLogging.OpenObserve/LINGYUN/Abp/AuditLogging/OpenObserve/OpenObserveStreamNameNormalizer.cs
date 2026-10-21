using System;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.AuditLogging.OpenObserve;

/// <summary>
/// OpenObserve 数据流名称规范器
/// </summary>
/// <remarks>
/// 租户数据流形如 <c>audit_log_{tenantId:N}</c>, 宿主(未指定租户)使用基础名称 <c>audit_log</c>。
/// 写入时显式传入日志自身的租户(审计日志写入发生在后台队列, 当前租户可能已不是产生日志的租户),
/// 查询时使用当前租户
/// </remarks>
public class OpenObserveStreamNameNormalizer : IOpenObserveStreamNameNormalizer, ISingletonDependency
{
    /// <summary>
    /// 租户分隔符
    /// </summary>
    /// <remarks>
    /// OpenObserve 会把数据流名称中的 <c>-</c> 规范化为 <c>_</c>(实测),
    /// 因此这里直接使用 <c>_</c>, 避免写入与查询的名称不一致
    /// </remarks>
    public const char TenantSeparator = '_';

    protected ICurrentTenant CurrentTenant { get; }

    public OpenObserveStreamNameNormalizer(ICurrentTenant currentTenant)
    {
        CurrentTenant = currentTenant;
    }

    public virtual string NormalizeStream(string stream, Guid? tenantId = null)
    {
        if (stream.IsNullOrWhiteSpace())
        {
            return stream;
        }

        var tenant = tenantId ?? CurrentTenant.Id;

        return tenant.HasValue
            ? $"{stream}{TenantSeparator}{tenant.Value:N}"
            : stream;
    }
}

using System;

namespace LINGYUN.Abp.AuditLogging.OpenObserve;

/// <summary>
/// OpenObserve 数据流名称规范器
/// </summary>
/// <remarks>
/// 参考 Elasticsearch 实现的 <c>IIndexNameNormalizer</c>: 不同租户使用不同数据流,
/// 避免多租户数据混在一起
/// </remarks>
public interface IOpenObserveStreamNameNormalizer
{
    /// <summary>
    /// 规范化数据流名称(追加租户后缀)
    /// </summary>
    /// <param name="stream">基础数据流名称</param>
    /// <param name="tenantId">租户标识, 为空时使用当前租户</param>
    string NormalizeStream(string stream, Guid? tenantId = null);
}

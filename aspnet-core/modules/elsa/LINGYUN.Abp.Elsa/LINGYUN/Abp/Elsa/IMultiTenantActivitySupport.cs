using System;

namespace LINGYUN.Abp.Elsa;
/// <summary>
/// 多租户支持
/// </summary>
/// <remarks>
/// elsa与abp的兼容，还是最好指定多租户字段
/// </remarks>
public interface IMultiTenantActivitySupport
{
    Guid? TenantId { get; set; }
}

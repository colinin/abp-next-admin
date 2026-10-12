namespace LINGYUN.Abp.AuditLogging.OpenObserve;

/// <summary>
/// 统计查询结果
/// </summary>
/// <remarks>
/// 对应 <c>select count(*) as total from ...</c> 的命中结构
/// </remarks>
public class OpenObserveCount
{
    public long Total { get; set; }
}

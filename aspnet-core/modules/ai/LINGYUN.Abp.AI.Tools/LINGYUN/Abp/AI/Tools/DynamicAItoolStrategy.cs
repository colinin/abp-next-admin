namespace LINGYUN.Abp.AI.Tools;
/// <summary>
/// 动态工具策略
/// </summary>
public enum DynamicAItoolStrategy : byte
{
    /// <summary>
    /// 忽略
    /// </summary>
    Ignore = 0,
    /// <summary>
    /// 覆盖
    /// </summary>
    Covering =1,
    /// <summary>
    /// 合并
    /// </summary>
    Merge = 2
}

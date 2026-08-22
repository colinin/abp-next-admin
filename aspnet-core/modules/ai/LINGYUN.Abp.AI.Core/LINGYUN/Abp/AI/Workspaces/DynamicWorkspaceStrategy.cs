namespace LINGYUN.Abp.AI.Workspaces;
/// <summary>
/// 动态工作区策略
/// </summary>
public enum DynamicWorkspaceStrategy
{
    /// <summary>
    /// 忽略动态工作区（静态优先）
    /// </summary>
    Ignore,

    /// <summary>
    /// 动态覆盖静态
    /// </summary>
    Covering,

    /// <summary>
    /// 合并策略
    /// </summary>
    Merge
}

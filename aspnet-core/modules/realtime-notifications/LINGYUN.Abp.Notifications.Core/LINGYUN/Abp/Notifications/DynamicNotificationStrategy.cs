namespace LINGYUN.Abp.Notifications;
/// <summary>
/// 动态通知策略
/// </summary>
public enum DynamicNotificationStrategy : byte
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

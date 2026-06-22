namespace LINGYUN.Abp.Webhooks;
/// <summary>
/// 动态Webhook策略
/// </summary>
public enum DynamicWebhookStrategy : byte
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

namespace LINGYUN.Abp.Notifications;
/// <summary>
/// 通知类关键字,请勿随意占用
/// </summary>
public static class NotificationKeywords
{
    /// <summary>
    /// 通知标识,全局唯一标识,long类型字段
    /// </summary>
    public const string Id = "notificationId";
    /// <summary>
    /// 通知名称
    /// </summary>
    public const string Name = "notification";
    /// <summary>
    /// 来源用户标识
    /// </summary>
    public const string FormUser = "formUser";
    /// <summary>
    /// 目标用户标识
    /// </summary>
    public const string ToUser = "toUser";
    /// <summary>
    /// 通知标题
    /// </summary>
    public const string Title = "title";
    /// <summary>
    /// 创建时间
    /// </summary>
    public const string CreationTime = "creationTime";
}

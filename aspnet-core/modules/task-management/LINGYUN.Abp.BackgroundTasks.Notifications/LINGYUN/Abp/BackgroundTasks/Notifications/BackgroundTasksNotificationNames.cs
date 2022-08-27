namespace LINGYUN.Abp.BackgroundTasks.Notifications;

public static class BackgroundTasksNotificationNames
{
    public const string GroupName = "Abp.Notifications.BackgroundTasks";
    /// <summary>
    /// 作业执行成功
    /// </summary>
    public const string JobExecuteSucceeded = GroupName + ".ExecuteSucceeded";
    /// <summary>
    /// 作业执行失败
    /// </summary>
    public const string JobExecuteFailed = GroupName + ".ExecuteFailed";
    /// <summary>
    /// 作业执行完毕
    /// </summary>
    public const string JobExecuteCompleted = GroupName + ".ExecuteCompleted";
}

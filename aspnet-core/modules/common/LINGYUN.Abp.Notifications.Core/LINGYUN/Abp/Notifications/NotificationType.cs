namespace LINGYUN.Abp.Notifications
{
    /// <summary>
    /// 通知类型
    /// </summary>
    public enum NotificationType
    {
        /// <summary>
        /// 应用（对应租户）
        /// </summary>
        Application = 0,
        /// <summary>
        /// 系统（对应宿主）
        /// </summary>
        System = 10,
        /// <summary>
        /// 用户（对应用户）
        /// </summary>
        User = 20
    }
}

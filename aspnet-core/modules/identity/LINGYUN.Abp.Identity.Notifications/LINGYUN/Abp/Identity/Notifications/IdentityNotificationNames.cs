namespace LINGYUN.Abp.Identity.Notifications;
public static class IdentityNotificationNames
{
    public const string GroupName = "AbpIdentity";

    public static class Session
    {
        public const string Prefix = GroupName + ".Session";
        /// <summary>
        /// 会话过期通知
        /// </summary>
        public const string ExpirationSession = Prefix + ".Expiration";
    }

    public static class IdentityUser
    {
        public const string Prefix = GroupName + ".IdentityUser";
        /// <summary>
        /// 账户保持活跃通知
        /// </summary>
        public const string InactiveUserReminderNotifier = Prefix + ".InactiveUserReminderNotifier";
        /// <summary>
        /// 账户停用通知
        /// </summary>
        public const string InactiveUserDeactivationNotifier = Prefix + ".InactiveUserDeactivationNotifier";
        /// <summary>
        /// 账户删除通知
        /// </summary>
        public const string InactiveUserDeletionNotifier = Prefix + ".InactiveUserDeletionNotifier";
    }
}

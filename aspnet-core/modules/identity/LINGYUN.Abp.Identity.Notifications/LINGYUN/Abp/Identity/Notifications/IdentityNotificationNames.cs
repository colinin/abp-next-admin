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
}

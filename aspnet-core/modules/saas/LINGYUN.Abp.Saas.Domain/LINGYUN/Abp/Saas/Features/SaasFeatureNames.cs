namespace LINGYUN.Abp.Saas.Features;
public static class SaasFeatureNames
{
    public const string GroupName = "AbpSaas";

    public static class Tenant
    {
        public const string Default = GroupName + ".Tenants";
        /// <summary>
        /// 资源回收策略
        /// </summary>
        public const string RecycleStrategy = Default + ".RecycleStrategy";
        /// <summary>
        /// 过期回收时长
        /// </summary>
        /// <remarks>
        /// 当资源快过期时, 需要邮件通知对应租户管理员, 超期多久没有处理, 将回收租户资源
        /// </remarks>
        public const string ExpiredRecoveryTime = Default + ".ExpiredRecoveryTime";
        /// <summary>
        /// 过期预警天数
        /// </summary>
        /// <remarks>
        /// 当资源还有多少天过期时, 给管理员发送提醒
        /// </remarks>
        public const string ExpirationReminderDays = Default + ".ExpirationReminderDays";
    }
}

namespace LINGYUN.Abp.Identity.Settings;

public static class IdentitySettingNames
{
    private const string Prefix = "Abp.Identity";
    public static class User
    {
        private const string UserPrefix = Prefix + ".User";
        /// <summary>
        /// 用户手机验证短信模板
        /// </summary>
        public const string SmsPhoneNumberConfirmed = UserPrefix + ".SmsPhoneNumberConfirmed";
        /// <summary>
        /// 用户注册短信验证码模板号
        /// </summary>
        public const string SmsNewUserRegister = UserPrefix + ".SmsNewUserRegister";
        /// <summary>
        /// 用户登录短信验证码模板号
        /// </summary>
        public const string SmsUserSignin = UserPrefix + ".SmsUserSignin";
        /// <summary>
        /// 用户重置密码短信验证码模板号
        /// </summary>
        public const string SmsResetPassword = UserPrefix + ".SmsResetPassword";
        /// <summary>
        /// 短信验证码重复间隔时间
        /// </summary>
        public const string SmsRepetInterval = UserPrefix + ".SmsRepetInterval";
    }

    public static class Session
    {
        private const string SessionPrefix = Prefix + ".Session";
        /// <summary>
        /// 并发登录策略
        /// see: https://github.com/abpio/abp-commercial-docs/blob/dev/en/modules/identity/session-management.md#prevent-concurrent-login
        /// </summary>
        public const string ConcurrentLoginStrategy = SessionPrefix + ".ConcurrentLoginStrategy";
        /// <summary>
        /// 限制相同设备登录数量
        /// </summary>
        public const string LogoutFromSameTypeDevicesLimit = SessionPrefix + ".LogoutFromSameTypeDevicesLimit";
    }
}

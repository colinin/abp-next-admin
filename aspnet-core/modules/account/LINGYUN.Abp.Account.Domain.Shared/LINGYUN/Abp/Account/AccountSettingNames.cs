namespace LINGYUN.Abp.Account
{
    public class AccountSettingNames
    {
        public const string GroupName = "Abp.Account";
        /// <summary>
        /// 短信验证码过期时间
        /// </summary>
        public const string PhoneVerifyCodeExpiration = GroupName + ".PhoneVerifyCodeExpiration";
        /// <summary>
        /// 用户注册短信验证码模板号
        /// </summary>
        public const string SmsRegisterTemplateCode = GroupName + ".SmsRegisterTemplateCode";
        /// <summary>
        /// 用户登录短信验证码模板号
        /// </summary>
        public const string SmsSigninTemplateCode = GroupName + ".SmsSigninTemplateCode";
        /// <summary>
        /// 用户重置密码短信验证码模板号
        /// </summary>
        public const string SmsResetPasswordTemplateCode = GroupName + ".SmsResetPasswordTemplateCode";
    }
}

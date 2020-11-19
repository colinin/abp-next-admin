namespace LINGYUN.Abp.SettingManagement
{
    public class PasswordDto
    {
        /// <summary>
        /// 密码的最小长度
        /// </summary>
        public SettingDetailsDto RequiredLength { get; set; }
        /// <summary>
        /// 密码必须包含唯一字符的数量
        /// </summary>
        public SettingDetailsDto RequiredUniqueChars { get; set; }
        /// <summary>
        /// 密码是否必须包含非字母数字
        /// </summary>
        public SettingDetailsDto RequireNonAlphanumeric { get; set; }
        /// <summary>
        /// 密码是否必须包含小写字母
        /// </summary>
        public SettingDetailsDto RequireLowercase { get; set; }
        /// <summary>
        /// 密码是否必须包含大写字母
        /// </summary>
        public SettingDetailsDto RequireUppercase { get; set; }
        /// <summary>
        /// 密码是否必须包含数字
        /// </summary>
        public SettingDetailsDto RequireDigit { get; set; }
    }
    public class LockoutDto
    {
        /// <summary>
        /// 允许新用户被锁定
        /// </summary>
        public SettingDetailsDto AllowedForNewUsers { get; set; }
        /// <summary>
        /// 当锁定发生时用户被的锁定的时间(秒)
        /// </summary>
        public SettingDetailsDto LockoutDuration { get; set; }
        /// <summary>
        /// 如果启用锁定, 当用户被锁定前失败的访问尝试次数
        /// </summary>
        public SettingDetailsDto MaxFailedAccessAttempts { get; set; }
    }
    public class SignInDto
    {
        /// <summary>
        /// 登录时是否需要验证电子邮箱
        /// </summary>
        public SettingDetailsDto RequireConfirmedEmail { get; set; }
        /// <summary>
        /// 用户是否可以确认手机号码
        /// </summary>
        public SettingDetailsDto EnablePhoneNumberConfirmation { get; set; }
        /// <summary>
        /// 登录时是否需要验证手机号码
        /// </summary>
        public SettingDetailsDto RequireConfirmedPhoneNumber { get; set; }
    }
    public class UserDto
    {
        /// <summary>
        /// 是否允许用户更新用户名
        /// </summary>
        public SettingDetailsDto IsUserNameUpdateEnabled { get; set; }
        /// <summary>
        /// 是否允许用户更新电子邮箱
        /// </summary>
        public SettingDetailsDto IsEmailUpdateEnabled { get; set; }
        /// <summary>
        /// 用户注册短信验证码模板号
        /// </summary>
        public SettingDetailsDto SmsNewUserRegister { get; set; }
        /// <summary>
        /// 用户登录短信验证码模板号
        /// </summary>
        public SettingDetailsDto SmsUserSignin { get; set; }
        /// <summary>
        /// 用户重置密码短信验证码模板号
        /// </summary>
        public SettingDetailsDto SmsResetPassword { get; set; }
        /// <summary>
        /// 验证码重复间隔时间
        /// </summary>
        public SettingDetailsDto SmsRepetInterval { get; set; }
        /// <summary>
        /// 用户手机验证短信模板
        /// </summary>
        public SettingDetailsDto SmsPhoneNumberConfirmed { get; set; }
    }

    public class OrganizationUnitDto
    {
        /// <summary>
        /// 单个用户最大组织机构数量
        /// </summary>
        public SettingDetailsDto MaxUserMembershipCount { get; set; }
    }

    public class TwoFactorDto
    {
        /// <summary>
        /// 双因素认证行为
        /// </summary>
        public SettingDetailsDto Behaviour { get; set; }
        /// <summary>
        /// 用户是否可以变更双因素模式
        /// </summary>
        public SettingDetailsDto UsersCanChange { get; set; }
    }
}

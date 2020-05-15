using System;
using System.Collections.Generic;
using System.Text;

namespace LINGYUN.Abp.SettingManagement
{
    public class IdentitySettingsDto
    {
        public IdentityPasswordSettings Password { get; set; }
        public IdentityLockoutSettings Lockout { get; set; }
        public IdentitySignInSettings SignIn { get; set; }
        public IdentityUserSettings User { get; set; }
    }

    public class IdentityPasswordSettings
    {
        /// <summary>
        /// 密码的最小长度
        /// </summary>
        public int RequiredLength { get; set; }
        /// <summary>
        /// 密码必须包含唯一字符的数量
        /// </summary>
        public int RequiredUniqueChars { get; set; }
        /// <summary>
        /// 密码是否必须包含非字母数字
        /// </summary>
        public bool RequireNonAlphanumeric { get; set; }
        /// <summary>
        /// 密码是否必须包含小写字母
        /// </summary>
        public bool RequireLowercase { get; set; }
        /// <summary>
        /// 密码是否必须包含大写字母
        /// </summary>
        public bool RequireUppercase { get; set; }
        /// <summary>
        /// 密码是否必须包含数字
        /// </summary>
        public bool RequireDigit { get; set; }
    }
    public class IdentityLockoutSettings
    {
        /// <summary>
        /// 允许新用户被锁定
        /// </summary>
        public bool AllowedForNewUsers { get; set; }
        /// <summary>
        /// 当锁定发生时用户被的锁定的时间(秒)
        /// </summary>
        public int LockoutDuration { get; set; }
        /// <summary>
        /// 如果启用锁定, 当用户被锁定前失败的访问尝试次数
        /// </summary>
        public int MaxFailedAccessAttempts { get; set; }
    }
    public class IdentitySignInSettings
    {
        /// <summary>
        /// 登录时是否需要验证电子邮箱
        /// </summary>
        public bool RequireConfirmedEmail { get; set; }
        /// <summary>
        /// 用户是否可以确认手机号码
        /// </summary>
        public bool EnablePhoneNumberConfirmation { get; set; }
        /// <summary>
        /// 登录时是否需要验证手机号码
        /// </summary>
        public bool RequireConfirmedPhoneNumber { get; set; }
    }
    public class IdentityUserSettings
    {
        /// <summary>
        /// 是否允许用户更新用户名
        /// </summary>
        public bool IsUserNameUpdateEnabled { get; set; }
        /// <summary>
        /// 是否允许用户更新电子邮箱
        /// </summary>
        public bool IsEmailUpdateEnabled { get; set; }
    }
}

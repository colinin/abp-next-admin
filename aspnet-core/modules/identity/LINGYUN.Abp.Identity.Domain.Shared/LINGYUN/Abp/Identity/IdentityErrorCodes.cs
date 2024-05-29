namespace LINGYUN.Abp.Identity
{
    public class IdentityErrorCodes
    {
        /// <summary>
        /// 无法变更静态声明类型
        /// </summary>
        public const string StaticClaimTypeChange = "Volo.Abp.Identity:020005";
        /// <summary>
        /// 无法删除静态声明类型
        /// </summary>
        public const string StaticClaimTypeDeletion = "Volo.Abp.Identity:020006";
        /// <summary>
        /// 手机号码已被使用
        /// </summary>
        public const string DuplicatePhoneNumber = "Volo.Abp.Identity:020007";
        /// <summary>
        /// 你不能修改你的手机绑定信息
        /// </summary>
        public const string UsersCanNotChangePhoneNumber = "Volo.Abp.Identity:020008";
        /// <summary>
        /// 你不能修改你的邮件绑定信息
        /// </summary>
        public const string UsersCanNotChangeEmailAddress = "Volo.Abp.Identity:020009";
        /// <summary>
        /// 重复确认的邮件地址
        /// </summary>
        public const string DuplicateConfirmEmailAddress = "Volo.Abp.Identity:020010";
        /// <summary>
        /// 尝试在未绑定MFA设备时启用二次认证
        /// </summary>
        public const string ChangeTwoFactorWithMFANotBound = "Volo.Abp.Identity:020011";
        /// <summary>
        /// 验证器验证无效
        /// </summary>
        public const string AuthenticatorTokenInValid = "Volo.Abp.Identity:020012";
    }
}

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
    }
}

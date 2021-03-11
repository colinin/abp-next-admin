namespace LINGYUN.Abp.Identity
{
    public static class ExtensionIdentityUserConsts
    {
        /// <summary>
        /// 头像字段
        /// </summary>
        public static string AvatarUrlField { get; set; } = "AvatarUrl";
        /// <summary>
        /// 头像字段最大长度
        /// </summary>
        public static int MaxAvatarUrlLength { get; set; } = 128;
    }
}

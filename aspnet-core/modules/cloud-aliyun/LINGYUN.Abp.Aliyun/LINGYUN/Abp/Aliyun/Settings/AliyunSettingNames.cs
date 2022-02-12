namespace LINGYUN.Abp.Aliyun.Settings
{
    public static class AliyunSettingNames
    {
        public const string Prefix = "Abp.Aliyun";

        /// <summary>
        /// 认证方式
        /// </summary>
        public static class Authorization
        {
            public const string Prefix = AliyunSettingNames.Prefix + ".Authorization";
            /// <summary>
            /// 地域ID
            /// </summary>
            public const string RegionId = Prefix + ".RegionId";
            /// <summary>
            /// RAM账号的AccessKey ID
            /// </summary>
            public const string AccessKeyId = Prefix + ".AccessKeyId";
            /// <summary>
            /// RAM账号的AccessKey Secret
            /// </summary>
            public const string AccessKeySecret = Prefix + ".AccessKeySecret";
            /// <summary>
            /// 使用STS Token访问
            /// </summary>
            public const string UseSecurityTokenService = Prefix + ".UseSecurityTokenService";
            /// <summary>
            /// 使用RAM子账号的AssumeRole方式访问
            /// </summary>
            public const string RamRoleArn = Prefix + ".RamRoleArn";
            /// <summary>
            /// 用户自定义参数。此参数用来区分不同的令牌，可用于用户级别的访问审计
            /// </summary>
            public const string RoleSessionName = Prefix + ".RoleSessionName";
            /// <summary>
            /// 过期时间，单位为秒。
            /// </summary>
            public const string DurationSeconds = Prefix + ".DurationSeconds";
            /// <summary>
            /// 权限策略。
            /// </summary>
            public const string Policy = Prefix + ".Policy";
        }
    }
}

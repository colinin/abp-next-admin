namespace LINGYUN.Abp.Aliyun.Settings
{
    public static class AliyunSettingNames
    {
        public const string Prefix = "Abp.Aliyun";

        /// <summary>
        /// 认证方式
        /// </summary>
        public class Authorization
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

        /// <summary>
        /// 短信服务
        /// </summary>
        public class Sms
        {
            public const string Prefix = AliyunSettingNames.Prefix + ".Sms";
            /// <summary>
            /// 阿里云sms服务域名
            /// </summary>
            public const string Domain = Prefix + ".Domain";
            /// <summary>
            /// 调用方法名称
            /// </summary>
            public const string ActionName = Prefix + ".ActionName";
            /// <summary>
            /// 默认版本号
            /// </summary>
            public const string Version = Prefix + ".Version";
            /// <summary>
            /// 默认签名
            /// </summary>
            public const string DefaultSignName = Prefix + ".DefaultSignName";
            /// <summary>
            /// 默认短信模板号
            /// </summary>
            public const string DefaultTemplateCode = Prefix + ".DefaultTemplateCode";
            /// <summary>
            /// 默认号码
            /// </summary>
            public const string DefaultPhoneNumber = Prefix + ".DefaultPhoneNumber";
            /// <summary>
            /// 展示错误给客户端
            /// </summary>
            public const string VisableErrorToClient = Prefix + ".VisableErrorToClient";
        }
    }
}

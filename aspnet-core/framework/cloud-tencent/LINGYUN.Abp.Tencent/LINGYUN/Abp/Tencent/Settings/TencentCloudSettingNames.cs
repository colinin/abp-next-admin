namespace LINGYUN.Abp.Tencent.Settings;

public static class TencentCloudSettingNames
{
    public const string Prefix = "Abp.TencentCloud";
    /// <summary>
    /// SecretId
    /// </summary>
    public const string SecretId = Prefix + ".SecretId";
    /// <summary>
    /// SecretKey
    /// </summary>
    public const string SecretKey = Prefix + ".SecretKey";
    /// <summary>
    /// 连接地域
    /// </summary>
    public const string EndPoint = Prefix + ".EndPoint";
    /// <summary>
    /// 会话持续时间
    /// </summary>
    public const string DurationSecond = Prefix + ".DurationSecond";
    /// <summary>
    /// 认证方式
    /// </summary>
    public class Authorization
    {
        public const string Prefix = TencentCloudSettingNames.Prefix + ".Authorization";
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
    /// 连接设置
    /// </summary>
    public class Connection
    {
        public const string Prefix = TencentCloudSettingNames.Prefix + ".Connection";
        /// <summary>
        /// 代理服务器
        /// </summary>
        public const string WebProxy = Prefix + ".WebProxy";
        /// <summary>
        /// 连接地域
        /// 按照腾讯云文档，对于金融区服务需要指定域名
        /// </summary>
        public const string EndPoint = Prefix + ".EndPoint";
        /// <summary>
        /// Http方法，按照腾讯云文档，不同的方法对于请求大小有限制
        /// 默认：POST
        /// </summary>
        public const string HttpMethod = Prefix + ".HttpMethod";
        /// <summary>
        /// 超时时间
        /// </summary>
        public const string Timeout = Prefix + ".Timeout";
    }

    public static class Sms
    {
        private const string Prefix = TencentCloudSettingNames.Prefix + ".Sms";
        /// <summary>
        /// 短信 SdkAppId
        /// 在 短信控制台 添加应用后生成的实际 SdkAppId，示例如1400006666。
        /// </summary>
        public const string AppId = Prefix + ".AppId";
        /// <summary>
        /// 短信签名内容
        /// </summary>
        public const string DefaultSignName = Prefix + ".DefaultSignName";
        /// <summary>
        /// 默认短信模板 ID
        /// </summary>
        public const string DefaultTemplateId = Prefix + ".DefaultTemplateId";
    }
}

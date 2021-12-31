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
}

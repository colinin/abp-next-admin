namespace LINGYUN.Abp.OpenApi
{
    public class AppDescriptor
    {
        /// <summary>
        /// 应用名称
        /// </summary>
        public string AppName { get; set; }
        /// <summary>
        /// 应用标识
        /// </summary>
        public string AppKey { get; set; }
        /// <summary>
        /// 应用密钥
        /// </summary>
        public string AppSecret { get; set; }
        /// <summary>
        /// 应用token
        /// </summary>
        public string AppToken { get; set; }
        /// <summary>
        /// 签名有效时间
        /// 单位: s
        /// </summary>
        public int? SignLifetime { get; set; }

        public AppDescriptor() { }
        public AppDescriptor(
            string appName,
            string appKey,
            string appSecret,
            string appToken = null,
            int? signLifeTime = null)
        {
            AppName = appName;
            AppKey = appKey;
            AppSecret = appSecret;
            AppToken = appToken;
            SignLifetime = signLifeTime;
        }
    }
}

namespace LINGYUN.Abp.IdentityServer
{
    public class WeChatSignatureOptions
    {
        /// <summary>
        /// 微信服务器请求路径
        /// 填写在微信开发者中心配置的地址
        /// </summary>
        public string RequestPath { get; set; }
        /// <summary>
        /// 微信服务器请求token
        /// 填写在微信开发者中心配置的token
        /// </summary>
        public string Token { get; set; }
    }
}

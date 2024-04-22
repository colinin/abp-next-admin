namespace LINGYUN.Abp.WeChat.Official
{
    public class AbpWeChatOfficialOptions
    {
        /// <summary>
        /// 是否沙盒测试
        /// </summary>
        /// <remarks>
        /// Tips: 当使用测试号时，消息为明文传输，调试时需要启用
        /// </remarks>
        public bool IsSandBox { get; set; }
        /// <summary>
        /// 公众号服务器消息Url
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 公众号AppId
        /// </summary>
        public string AppId { get; set; }
        /// <summary>
        /// 公众号AppSecret
        /// </summary>
        public string AppSecret { get; set; }
        /// <summary>
        /// 公众号消息解密Token
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 公众号消息解密AESKey
        /// </summary>
        public string EncodingAESKey { get; set; }
    }
}

namespace LINGYUN.Abp.WeChat.MiniProgram
{
    public class AbpWeChatMiniProgramOptions
    {
        /// <summary>
        /// 小程序AppId
        /// </summary>
        public string AppId { get; set; }
        /// <summary>
        /// 小程序AppSecret
        /// </summary>
        public string AppSecret { get; set; }
        /// <summary>
        /// 小程序消息解密Token
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 小程序消息解密AESKey
        /// </summary>
        public string EncodingAESKey { get; set; }
    }
}

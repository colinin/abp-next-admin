namespace LINGYUN.Abp.WeChat.Token
{
    /// <summary>
    /// 微信令牌
    /// </summary>
    public class WeChatToken
    {
        /// <summary>
        /// 访问令牌
        /// </summary>
        public string AccessToken { get; set; }
        /// <summary>
        /// 过期时间,单位(s)
        /// </summary>
        public int ExpiresIn { get; set; }
        public WeChatToken()
        {

        }
        public WeChatToken(string token, int expiresIn)
        {
            AccessToken = token;
            ExpiresIn = expiresIn;
        }
    }
}

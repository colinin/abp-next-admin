namespace LINGYUN.Abp.WeChat.Work.Token.Models
{
    /// <summary>
    /// 企业微信令牌
    /// </summary>
    public class WeChatWorkToken
    {
        /// <summary>
        /// 访问令牌
        /// </summary>
        public string AccessToken { get; set; }
        /// <summary>
        /// 过期时间,单位(s)
        /// </summary>
        public int ExpiresIn { get; set; }
        public WeChatWorkToken()
        {

        }
        public WeChatWorkToken(string token, int expiresIn)
        {
            AccessToken = token;
            ExpiresIn = expiresIn;
        }
    }
}

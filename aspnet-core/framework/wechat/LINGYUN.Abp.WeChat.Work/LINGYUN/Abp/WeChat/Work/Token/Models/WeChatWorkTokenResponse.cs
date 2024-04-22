using Newtonsoft.Json;

namespace LINGYUN.Abp.WeChat.Work.Token.Models
{
    /// <summary>
    /// 微信访问令牌返回对象
    /// </summary>
    public class WeChatWorkTokenResponse : WeChatWorkResponse
    {
        /// <summary>
        /// 访问令牌
        /// </summary>
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        /// <summary>
        /// 过期时间,单位(s)
        /// </summary>
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        public WeChatWorkToken ToWeChatWorkToken()
        {
            ThrowIfNotSuccess();
            return new WeChatWorkToken(AccessToken, ExpiresIn);
        }
    }
}

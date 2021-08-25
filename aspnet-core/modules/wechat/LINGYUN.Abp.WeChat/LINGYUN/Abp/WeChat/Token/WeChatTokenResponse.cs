using Newtonsoft.Json;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Token
{
    /// <summary>
    /// 微信访问令牌返回对象
    /// </summary>
    public class WeChatTokenResponse
    {
        /// <summary>
        /// 错误码
        /// </summary>
        [JsonProperty("errcode")]
        public int ErrorCode { get; set; }
        /// <summary>
        /// 错误消息
        /// </summary>
        [JsonProperty("errmsg")]
        public string ErrorMessage { get; set; }
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

        public WeChatToken ToWeChatToken()
        {
            if(ErrorCode != 0)
            {
                throw new AbpException(ErrorMessage);
            }
            return new WeChatToken(AccessToken, ExpiresIn);
        }
    }
}

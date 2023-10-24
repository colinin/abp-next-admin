namespace LINGYUN.Abp.WeChat.OpenId
{
    public class WeChatOpenId
    {
        /// <summary>
        /// 用户在开放平台的唯一标识符，在满足 UnionID 下发条件的情况下会返回
        /// </summary>
        public string UnionId { get; set; }
        /// <summary>
        /// 用户唯一标识
        /// </summary>
        public string OpenId { get; set; }
        /// <summary>
        /// 会话密钥
        /// </summary>
        public string SessionKey { get; set; }

        public WeChatOpenId()
        {

        }

        public WeChatOpenId(string openId, string sessionKey, string unionId = null)
        {
            OpenId = openId;
            SessionKey = sessionKey;
            UnionId = unionId;
        }
    }
}

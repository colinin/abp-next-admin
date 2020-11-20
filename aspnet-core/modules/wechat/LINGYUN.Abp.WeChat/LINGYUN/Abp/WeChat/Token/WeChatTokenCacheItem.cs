namespace LINGYUN.Abp.WeChat.Token
{
    public class WeChatTokenCacheItem
    {
        public string AppId { get; set; }

        public WeChatToken WeChatToken { get; set; }
        public WeChatTokenCacheItem()
        {

        }

        public WeChatTokenCacheItem(string appId, WeChatToken weChatToken)
        {
            AppId = appId;
            WeChatToken = weChatToken;
        }

        public static string CalculateCacheKey(string provider, string appId)
        {
            return "p:" + provider + ",o:" + appId;
        }
    }
}

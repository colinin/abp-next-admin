namespace LINGYUN.Abp.WeChat.Work.Token.Models
{
    public class WeChatWorkTokenCacheItem
    {
        public string CorpId { get; set; }

        public string AgentId { get; set; }

        public WeChatWorkToken Token { get; set; }

        public WeChatWorkTokenCacheItem()
        {

        }

        public WeChatWorkTokenCacheItem(string corpId, string agentId, WeChatWorkToken token)
        {
            CorpId = corpId;
            AgentId = agentId;
            Token = token;
        }

        public static string CalculateCacheKey(string provider, string corpId, string agentId)
        {
            return "p:" + provider + ",cp:" + corpId + ",ag:" + agentId;
        }
    }
}

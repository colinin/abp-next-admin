namespace Microsoft.AspNetCore.Authentication.WeChat.Official
{
    public class WeChatOfficialStateCacheItem
    {
        public string State { get; set; }

        public WeChatOfficialStateCacheItem() { }
        public WeChatOfficialStateCacheItem(string state)
        {
            State = state;
        }

        public static string CalculateCacheKey(string correlationId, string purpose)
        {
            return $"ci:{correlationId};p:{purpose ?? "null"}";
        }
    }
}

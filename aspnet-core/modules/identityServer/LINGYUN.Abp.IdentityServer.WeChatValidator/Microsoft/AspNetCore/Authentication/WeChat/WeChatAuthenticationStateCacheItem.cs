namespace Microsoft.AspNetCore.Authentication.WeChat
{
    public class WeChatAuthenticationStateCacheItem
    {
        public string State { get; set; }

        public WeChatAuthenticationStateCacheItem() { }
        public WeChatAuthenticationStateCacheItem(string state)
        {
            State = state;
        }

        public static string CalculateCacheKey(string correlationId, string purpose)
        {
            return $"ci:{correlationId};p:{purpose ?? "null"}";
        }
    }
}

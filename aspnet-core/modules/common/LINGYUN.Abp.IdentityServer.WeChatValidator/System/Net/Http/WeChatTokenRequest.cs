namespace System.Net.Http
{
    public class WeChatTokenRequest
    {
        public string BaseUrl { get; set; }
        public string AppId { get; set; }
        public string Secret { get; set; }
        public string Code { get; set; }
    }
}

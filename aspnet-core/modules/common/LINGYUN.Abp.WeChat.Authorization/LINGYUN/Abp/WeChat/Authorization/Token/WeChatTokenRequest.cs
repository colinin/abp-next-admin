namespace LINGYUN.Abp.WeChat.Authorization
{
    public class WeChatTokenRequest
    {
        public string BaseUrl { get; set; }
        public string GrantType { get; set; }
        public string AppId { get; set; }
        public string AppSecret { get; set; }
    }
}

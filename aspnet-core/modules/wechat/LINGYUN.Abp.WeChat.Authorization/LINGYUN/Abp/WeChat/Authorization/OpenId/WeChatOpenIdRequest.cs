namespace LINGYUN.Abp.WeChat.Authorization
{
    public class WeChatOpenIdRequest
    {
        public string BaseUrl { get; set; }
        public string AppId { get; set; }
        public string Secret { get; set; }
        public string Code { get; set; }
    }
}

namespace LINGYUN.Abp.WeChat.OpenId
{
    public class WeChatOpenIdRequest
    {
        public string BaseUrl { get; set; }
        public string AppId { get; set; }
        public string Secret { get; set; }
        public string Code { get; set; }
    }
}

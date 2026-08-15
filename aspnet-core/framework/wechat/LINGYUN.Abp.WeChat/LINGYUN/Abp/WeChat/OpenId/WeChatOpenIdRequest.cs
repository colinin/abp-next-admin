namespace LINGYUN.Abp.WeChat.OpenId;

public class WeChatOpenIdRequest
{
    public string BaseUrl { get; set; } = default!;
    public string AppId { get; set; } = default!;
    public string Secret { get; set; } = default!;
    public string Code { get; set; } = default!;
}

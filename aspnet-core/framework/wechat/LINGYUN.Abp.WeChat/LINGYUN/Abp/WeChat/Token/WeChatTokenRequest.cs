namespace LINGYUN.Abp.WeChat.Token;

public class WeChatTokenRequest
{
    public string BaseUrl { get; set; } = default!;
    public string GrantType { get; set; } = default!;
    public string AppId { get; set; } = default!;
    public string AppSecret { get; set; } = default!;
}

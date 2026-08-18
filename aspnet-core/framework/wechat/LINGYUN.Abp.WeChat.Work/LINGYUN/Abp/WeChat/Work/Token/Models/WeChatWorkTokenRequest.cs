namespace LINGYUN.Abp.WeChat.Work.Token.Models;

public class WeChatWorkTokenRequest
{
    public string CorpId { get; set; }
    public string CorpSecret { get; set; }
    public WeChatWorkTokenRequest(string corpId, string corpSecret)
    {
        CorpId = corpId;
        CorpSecret = corpSecret;
    }
}

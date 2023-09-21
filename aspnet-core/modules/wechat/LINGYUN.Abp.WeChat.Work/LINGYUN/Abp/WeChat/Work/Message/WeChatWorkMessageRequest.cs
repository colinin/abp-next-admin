namespace LINGYUN.Abp.WeChat.Work.Message;
public class WeChatWorkMessageRequest
{
    public string AccessToken { get; set; }
    public WeChatWorkMessage Message { get; set; } 
    public WeChatWorkMessageRequest(string accessToken, WeChatWorkMessage message)
    {
        AccessToken = accessToken;
        Message = message;
    }
}

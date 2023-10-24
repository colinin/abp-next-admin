using Volo.Abp.Content;

namespace LINGYUN.Abp.WeChat.Work.Media.Models;
public class WeChatWorkMediaRequest
{
    public string AccessToken { get; set; }
    public IRemoteStreamContent Content { get; set; }
    public WeChatWorkMediaRequest(
        string accessToken,
        IRemoteStreamContent content)
    {
        AccessToken = accessToken;
        Content = content;
    }
}

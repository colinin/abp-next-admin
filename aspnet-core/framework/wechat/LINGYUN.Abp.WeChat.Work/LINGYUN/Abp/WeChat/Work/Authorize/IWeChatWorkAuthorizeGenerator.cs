using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.Work.Authorize;
public interface IWeChatWorkAuthorizeGenerator
{
    /// <summary>
    /// 构造网页授权链接
    /// </summary>
    /// <remarks>
    /// 参考：<see href="https://developer.work.weixin.qq.com/document/path/91022"/>
    /// </remarks>
    /// <param name="redirectUri"></param>
    /// <param name="state"></param>
    /// <param name="responseType"></param>
    /// <param name="scope"></param>
    /// <returns></returns>
    Task<string> GenerateOAuth2AuthorizeAsync(
        string redirectUri,
        string state,
        string responseType = "code",
        string scope = "snsapi_base");
    /// <summary>
    /// 构建网页登录链接
    /// </summary>
    /// <param name="redirectUri"></param>
    /// <param name="state"></param>
    /// <param name="loginType"></param>
    /// <param name="agentId"></param>
    /// <param name="lang"></param>
    /// <returns></returns>
    Task<string> GenerateOAuth2LoginAsync(
        string redirectUri,
        string state,
        string loginType = "ServiceApp",
        string agentId = "",
        string lang = "zh");
}

using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.WeChat.Work.Authorize;
public interface IWeChatWorkAuthorizeAppService : IApplicationService
{
    /// <summary>
    /// 生成授权链接
    /// </summary>
    /// <param name="urlName">授权回调Url名称</param>
    /// <param name="responseType">响应类型</param>
    /// <param name="scope">授权范围</param>
    /// <returns></returns>
    Task<string> GenerateOAuth2AuthorizeAsync(
        string urlName,
        string responseType = "code",
        string scope = "snsapi_base");
    /// <summary>
    /// 生成登录链接
    /// </summary>
    /// <param name="urlName">授权回调Url名称</param>
    /// <param name="loginType">登录类型</param>
    /// <returns></returns>
    Task<string> GenerateOAuth2LoginAsync(
        string urlName,
        string loginType = "CorpApp");
}

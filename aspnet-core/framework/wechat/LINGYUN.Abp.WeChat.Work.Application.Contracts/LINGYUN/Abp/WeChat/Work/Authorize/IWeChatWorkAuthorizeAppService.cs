using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.WeChat.Work.Authorize;
public interface IWeChatWorkAuthorizeAppService : IApplicationService
{
    Task<string> GenerateOAuth2AuthorizeAsync(
        string agentid,
        string redirectUri,
        string responseType = "code",
        string scope = "snsapi_base");

    Task<string> GenerateOAuth2LoginAsync(
        string redirectUri,
        string loginType = "ServiceApp",
        string agentid = "");
}

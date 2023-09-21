using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.WeChat.Work.Authorize;

[Controller]
[RemoteService(Name = AbpWeChatWorkRemoteServiceConsts.RemoteServiceName)]
[Area(AbpWeChatWorkRemoteServiceConsts.ModuleName)]
[Route("api/wechat/work/authorize")]
public class WeChatWorkAuthorizeController : AbpControllerBase, IWeChatWorkAuthorizeAppService
{
    private readonly IWeChatWorkAuthorizeAppService _service;

    public WeChatWorkAuthorizeController(IWeChatWorkAuthorizeAppService service)
    {
        _service = service;
    }

    [HttpGet]
    [Route("oauth2")]
    public virtual Task<string> GenerateOAuth2AuthorizeAsync(
        [FromQuery(Name = "agent_id")] string agentid,
        [FromQuery(Name = "redirect_uri")] string redirectUri,
        [FromQuery(Name = "response_type")] string responseType = "code",
        [FromQuery] string scope = "snsapi_base")
    {
        return _service.GenerateOAuth2AuthorizeAsync(agentid, redirectUri, responseType, scope);
    }

    [HttpGet]
    [Route("oauth2/login")]
    public virtual Task<string> GenerateOAuth2LoginAsync(
        [FromQuery] string appid,
        [FromQuery(Name = "redirect_uri")] string redirectUri,
        [FromQuery(Name = "login_type")] string loginType = "ServiceApp",
        [FromQuery(Name = "agent_id")] string agentid = "")
    {
        return _service.GenerateOAuth2LoginAsync(agentid, redirectUri, loginType, agentid);
    }
}

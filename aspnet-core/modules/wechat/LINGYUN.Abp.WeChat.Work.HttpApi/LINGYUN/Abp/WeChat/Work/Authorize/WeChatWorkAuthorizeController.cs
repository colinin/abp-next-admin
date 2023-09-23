using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Auditing;

namespace LINGYUN.Abp.WeChat.Work.Authorize;

[DisableAuditing]
[RemoteService(false)]
[ApiExplorerSettings(IgnoreApi = true)]
[Route("api/wechat/work/authorize")]
[Area(AbpWeChatWorkRemoteServiceConsts.ModuleName)]
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
    [Route("oauth2/authorize")]
    public async virtual Task<IActionResult> OAuth2AuthorizeAsync(
        [FromQuery(Name = "agent_id")] string agentid,
        [FromQuery(Name = "redirect_uri")] string redirectUri,
        [FromQuery(Name = "response_type")] string responseType = "code",
        [FromQuery] string scope = "snsapi_base")
    {
        var url = await _service.GenerateOAuth2AuthorizeAsync(agentid, redirectUri, responseType, scope);

        return Redirect(url);
    }

    [HttpGet]
    [Route("oauth2/login")]
    public virtual Task<string> GenerateOAuth2LoginAsync(
        [FromQuery(Name = "redirect_uri")] string redirectUri,
        [FromQuery(Name = "login_type")] string loginType = "ServiceApp",
        [FromQuery(Name = "agent_id")] string agentid = "")
    {
        return _service.GenerateOAuth2LoginAsync(redirectUri, loginType, agentid);
    }

    [HttpGet]
    [Route("oauth2/login/sso")]
    public async virtual Task<IActionResult> OAuth2LoginAsync(
        [FromQuery(Name = "redirect_uri")] string redirectUri,
        [FromQuery(Name = "login_type")] string loginType = "ServiceApp",
        [FromQuery(Name = "agent_id")] string agentid = "")
    {
        var url = await _service.GenerateOAuth2LoginAsync(redirectUri, loginType, agentid);

        return Redirect(url);
    }
}

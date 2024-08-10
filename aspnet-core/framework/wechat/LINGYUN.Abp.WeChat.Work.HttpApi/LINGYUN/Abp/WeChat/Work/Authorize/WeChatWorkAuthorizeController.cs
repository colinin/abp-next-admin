using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Auditing;

namespace LINGYUN.Abp.WeChat.Work.Authorize;

/// <summary>
/// 企业微信网页授权
/// </summary>
/// <remarks>
/// 详情见企业微信文档: <see cref="https://developer.work.weixin.qq.com/document/path/91335"/>
/// </remarks>
[Controller]
[DisableAuditing]
//[RemoteService(false)]
//[ApiExplorerSettings(IgnoreApi = true)]
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

    /// <summary>
    /// 构造网页授权链接
    /// </summary>
    /// <remarks>
    /// 详情见企业微信文档: <see cref="https://developer.work.weixin.qq.com/document/path/91022"/>
    /// </remarks>
    /// <param name="agentid">企业内部应用标识</param>
    /// <param name="redirectUri">登录成功重定向url</param>
    /// <param name="responseType">oauth响应类型</param>
    /// <param name="scope">oauth授权范围</param>
    /// <returns></returns>
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

    /// <summary>
    /// 构造网页授权链接
    /// </summary>
    /// <remarks>
    /// 详情见企业微信文档: <see cref="https://developer.work.weixin.qq.com/document/path/91022"/>
    /// </remarks>
    /// <param name="agentid">企业内部应用标识</param>
    /// <param name="redirectUri">登录成功重定向url</param>
    /// <param name="responseType">oauth响应类型</param>
    /// <param name="scope">oauth授权范围</param>
    /// <returns></returns>
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

    /// <summary>
    /// 构造企业微信登录链接
    /// </summary>
    /// <remarks>
    /// 详情见企业微信文档: <see cref="https://developer.work.weixin.qq.com/document/path/98152#api%E6%8E%A5%E5%8F%A3"/>
    /// </remarks>
    /// <param name="redirectUri">登录成功重定向url</param>
    /// <param name="loginType">登录类型, ServiceApp：服务商登录；CorpApp：企业自建/代开发应用登录</param>
    /// <param name="agentid">企业自建应用/服务商代开发应用 AgentID，当login_type=CorpApp时填写</param>
    /// <returns></returns>
    [HttpGet]
    [Route("oauth2/login")]
    public virtual Task<string> GenerateOAuth2LoginAsync(
        [FromQuery(Name = "redirect_uri")] string redirectUri,
        [FromQuery(Name = "login_type")] string loginType = "ServiceApp",
        [FromQuery(Name = "agent_id")] string agentid = "")
    {
        return _service.GenerateOAuth2LoginAsync(redirectUri, loginType, agentid);
    }

    /// <summary>
    /// 构造企业微信登录链接
    /// </summary>
    /// <remarks>
    /// 详情见企业微信文档: <see cref="https://developer.work.weixin.qq.com/document/path/98152#api%E6%8E%A5%E5%8F%A3"/>
    /// </remarks>
    /// <param name="redirectUri">登录成功重定向url</param>
    /// <param name="loginType">登录类型, ServiceApp：服务商登录；CorpApp：企业自建/代开发应用登录</param>
    /// <param name="agentid">企业自建应用/服务商代开发应用 AgentID，当login_type=CorpApp时填写</param>
    /// <returns></returns>
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

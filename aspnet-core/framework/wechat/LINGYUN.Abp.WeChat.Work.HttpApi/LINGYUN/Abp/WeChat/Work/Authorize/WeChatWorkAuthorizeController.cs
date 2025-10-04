using Microsoft.AspNetCore.Authorization;
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
    /// <param name="urlName">授权回调Url名称</param>
    /// <param name="responseType">oauth响应类型</param>
    /// <param name="scope">oauth授权范围</param>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    [Route("oauth2/generate")]
    public virtual Task<string> GenerateOAuth2AuthorizeAsync(
        [FromQuery] string urlName,
        [FromQuery(Name = "response_type")] string responseType = "code",
        [FromQuery] string scope = "snsapi_base")
    {
        return _service.GenerateOAuth2AuthorizeAsync(responseType, scope);
    }

    /// <summary>
    /// 构造网页授权链接
    /// </summary>
    /// <remarks>
    /// 详情见企业微信文档: <see cref="https://developer.work.weixin.qq.com/document/path/91022"/>
    /// </remarks>
    /// <param name="urlName">授权回调Url名称</param>
    /// <param name="responseType">oauth响应类型</param>
    /// <param name="scope">oauth授权范围</param>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    [Route("oauth2")]
    public async virtual Task<IActionResult> OAuth2AuthorizeAsync(
        [FromQuery] string urlName,
        [FromQuery(Name = "response_type")] string responseType = "code",
        [FromQuery] string scope = "snsapi_base")
    {
        var url = await _service.GenerateOAuth2AuthorizeAsync(urlName, responseType, scope);

        return Redirect(url);
    }

    /// <summary>
    /// 构造企业微信登录链接
    /// </summary>
    /// <remarks>
    /// 详情见企业微信文档: <see cref="https://developer.work.weixin.qq.com/document/path/98152#api%E6%8E%A5%E5%8F%A3"/>
    /// </remarks>
    /// <param name="urlName">授权回调Url名称</param>
    /// <param name="loginType">登录类型, ServiceApp：服务商登录；CorpApp：企业自建/代开发应用登录</param>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    [Route("oauth2/login/generate")]
    public virtual Task<string> GenerateOAuth2LoginAsync(
        string urlName,
        string loginType = "CorpApp")
    {
        return _service.GenerateOAuth2LoginAsync(urlName, loginType);
    }

    /// <summary>
    /// 构造企业微信登录链接
    /// </summary>
    /// <remarks>
    /// 详情见企业微信文档: <see cref="https://developer.work.weixin.qq.com/document/path/98152#api%E6%8E%A5%E5%8F%A3"/>
    /// </remarks>
    /// <param name="urlName">授权回调Url名称</param>
    /// <param name="loginType">登录类型, ServiceApp：服务商登录；CorpApp：企业自建/代开发应用登录</param>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    [Route("oauth2/login")]
    public async virtual Task<IActionResult> OAuth2LoginAsync(
        string urlName,
        string loginType = "CorpApp")
    {
        var url = await _service.GenerateOAuth2LoginAsync(urlName, loginType);

        return Redirect(url);
    }
}

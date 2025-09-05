using LINGYUN.Abp.WeChat.Work.JsSdk.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Auditing;

namespace LINGYUN.Abp.WeChat.Work.JsSdk;

// [Authorize]
[Controller]
[DisableAuditing]
[Route("api/wechat/work/jssdk")]
[Area(AbpWeChatWorkRemoteServiceConsts.ModuleName)]
[RemoteService(Name = AbpWeChatWorkRemoteServiceConsts.RemoteServiceName)]
public class WeChatWorkJsSdkController : AbpControllerBase, IWeChatWorkJsSdkAppService
{
    private readonly IWeChatWorkJsSdkAppService _service;

    public WeChatWorkJsSdkController(IWeChatWorkJsSdkAppService service)
    {
        _service = service;
    }

    [HttpGet]
    [Route("agent-config")]
    public virtual Task<AgentConfigDto> GetAgentConfigAsync()
    {
        return _service.GetAgentConfigAsync();
    }

    [HttpGet]
    [Route("agent-signature")]
    public virtual Task<JsApiSignatureDto> GetAgentSignatureAsync(string url)
    {
        return _service.GetAgentSignatureAsync(url);
    }

    [HttpGet]
    [Route("signature")]
    public virtual Task<JsApiSignatureDto> GetSignatureAsync(string url)
    {
        return _service.GetSignatureAsync(url);
    }
}

using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Auditing;

namespace LINGYUN.Abp.WeChat.Work.Message;

/// <summary>
/// 企业微信消息处理接口
/// </summary>
/// <remarks>
/// 详情见企业微信文档: <seealso cref="https://developer.work.weixin.qq.com/document/path/90930"/>
/// </remarks>
[Controller]
[DisableAuditing]
[RemoteService(Name = AbpWeChatWorkRemoteServiceConsts.RemoteServiceName)]
[Area(AbpWeChatWorkRemoteServiceConsts.ModuleName)]
[Route("api/wechat/work/messages")]
public class WeChatWorkMessageController : AbpControllerBase, IWeChatWorkMessageAppService
{
    private readonly IWeChatWorkMessageAppService _service;

    public WeChatWorkMessageController(IWeChatWorkMessageAppService service)
    {
        _service = service;
    }

    /// <summary>
    /// 企业微信服务器消息验证回调接口<br />
    /// 验证企业微信发送的消息有效性
    /// </summary>
    /// <param name="agentId">企业内部应用标识</param>
    /// <param name="input">企业微信服务器传递的验证消息参数</param>
    /// <returns></returns>
    [HttpGet]
    [Route("{agentId}")]
    public virtual Task<string> Handle([FromRoute] string agentId, [FromQuery] MessageValidationInput input)
    {
        return _service.Handle(agentId, input);
    }

    /// <summary>
    /// 企业微信服务器业务消息回调接口<br />
    /// 处理企业微信服务器发送的业务消息
    /// </summary>
    /// <param name="agentId">企业内部应用标识</param>
    /// <param name="input">企业微信服务器传递的消息参数</param>
    /// <returns></returns>
    [HttpPost]
    [Route("{agentId}")]
    public async virtual Task<string> Handle([FromRoute] string agentId, [FromQuery] MessageHandleInput input)
    {
        using var reader = new StreamReader(Request.Body, Encoding.UTF8);
        var content = await reader.ReadToEndAsync();

        input.Data = content;

        return await _service.Handle(agentId, input);
    }
}

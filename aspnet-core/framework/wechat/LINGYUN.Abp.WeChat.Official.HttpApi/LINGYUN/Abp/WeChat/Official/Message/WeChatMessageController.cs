using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.WeChat.Official.Message;

/// <summary>
/// 微信公众号消息处理接口
/// </summary>
/// <remarks>
/// 详情见微信公众号文档: <seealso cref="https://developers.weixin.qq.com/doc/offiaccount/Basic_Information/Access_Overview.html"/>
/// </remarks>
[Controller]
[RemoteService(Name = AbpWeChatOfficialRemoteServiceConsts.RemoteServiceName)]
[Area(AbpWeChatOfficialRemoteServiceConsts.ModuleName)]
[Route("api/wechat/official/messages")]
public class WeChatMessageController : AbpControllerBase, IWeChatMessageAppService
{
    private readonly IWeChatMessageAppService _service;

    public WeChatMessageController(IWeChatMessageAppService service)
    {
        _service = service;
    }
    /// <summary>
    /// 微信公众号服务器消息验证回调接口<br />
    /// 验证微信公众号服务器发送的消息有效性
    /// </summary>
    /// <param name="input">微信公众号服务器传递的验证消息参数</param>
    /// <returns></returns>
    [HttpGet]
    public virtual Task<string> Handle([FromQuery] MessageValidationInput input)
    {
        return _service.Handle(input);
    }

    /// <summary>
    /// 微信公众号服务器业务消息回调接口<br />
    /// 处理微信公众号服务器发送的业务消息
    /// </summary>
    /// <param name="input">微信公众号服务器传递的消息参数</param>
    /// <returns></returns>
    [HttpPost]
    public async virtual Task<string> Handle([FromQuery] MessageHandleInput input)
    {
        using var reader = new StreamReader(Request.Body, Encoding.UTF8);
        var content = await reader.ReadToEndAsync();

        input.Data = content;

        return await _service.Handle(input);
    }
}

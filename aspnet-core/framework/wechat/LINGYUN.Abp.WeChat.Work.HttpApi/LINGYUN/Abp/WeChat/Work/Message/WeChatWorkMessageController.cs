using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.WeChat.Work.Message;

[Controller]
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

    [HttpGet]
    [Route("{agentId}")]
    public virtual Task<string> Handle([FromRoute] string agentId, [FromQuery] MessageValidationInput input)
    {
        return _service.Handle(agentId, input);
    }

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

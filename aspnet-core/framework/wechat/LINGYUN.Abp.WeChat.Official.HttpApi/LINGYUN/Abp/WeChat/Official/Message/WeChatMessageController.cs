using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.WeChat.Official.Message;

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

    [HttpGet]
    public virtual Task<string> Handle([FromQuery] MessageValidationInput input)
    {
        return _service.Handle(input);
    }

    [HttpPost]
    public async virtual Task<string> Handle([FromQuery] MessageHandleInput input)
    {
        using var reader = new StreamReader(Request.Body, Encoding.UTF8);
        var content = await reader.ReadToEndAsync();

        input.Data = content;

        return await _service.Handle(input);
    }
}

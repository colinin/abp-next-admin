using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Content;

namespace LINGYUN.Abp.WeChat.Official.Account;

[Controller]
[RemoteService(Name = AbpWeChatOfficialRemoteServiceConsts.RemoteServiceName)]
[Area(AbpWeChatOfficialRemoteServiceConsts.ModuleName)]
[Route("api/wechat/official/account/parametric-qrcode")]
public class ParametricQrCodeController : AbpControllerBase, IParametricQrCodeAppService
{
    private readonly IParametricQrCodeAppService _service;

    public ParametricQrCodeController(IParametricQrCodeAppService service)
    {
        _service = service;
    }

    [HttpPost]
    [Route("generate")]
    public virtual Task<IRemoteStreamContent> GenerateAsync(ParametricQrCodeGenerateInput input)
    {
        return _service.GenerateAsync(input);
    }
}

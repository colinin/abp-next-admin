using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Content;

namespace LINGYUN.Abp.WeChat.Official.Account;

/// <summary>
/// 带参数二维码接口
/// </summary>
/// <remarks>
/// 详情见: https://developers.weixin.qq.com/doc/offiaccount/Account_Management/Generating_a_Parametric_QR_Code.html
/// </remarks>
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
    /// <summary>
    /// 生成带参数的二维码
    /// </summary>
    /// <remarks>
    /// 详情见: https://developers.weixin.qq.com/doc/offiaccount/Account_Management/Generating_a_Parametric_QR_Code.html
    /// </remarks>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("generate")]
    public virtual Task<IRemoteStreamContent> GenerateAsync(ParametricQrCodeGenerateInput input)
    {
        return _service.GenerateAsync(input);
    }
}

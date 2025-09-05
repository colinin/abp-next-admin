using LINGYUN.Abp.WeChat.Work.Features;
using LINGYUN.Abp.WeChat.Work.JsSdk.Dtos;
using LINGYUN.Abp.WeChat.Work.Settings;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Web;
using Volo.Abp.Application.Services;
using Volo.Abp.Features;

namespace LINGYUN.Abp.WeChat.Work.JsSdk;

[Authorize]
[RequiresFeature(WeChatWorkFeatureNames.Enable)]
public class WeChatWorkJsSdkAppService : ApplicationService, IWeChatWorkJsSdkAppService
{
    private readonly IJsApiTicketProvider _ticketProvider;

    public WeChatWorkJsSdkAppService(IJsApiTicketProvider ticketProvider)
    {
        _ticketProvider = ticketProvider;
    }

    public async virtual Task<AgentConfigDto> GetAgentConfigAsync()
    {
        var corpId = await SettingProvider.GetOrNullAsync(WeChatWorkSettingNames.Connection.CorpId);
        var agentId = await SettingProvider.GetOrNullAsync(WeChatWorkSettingNames.Connection.AgentId);

        return new AgentConfigDto
        {
            CorpId = corpId,
            AgentId = agentId,
        };
    }

    public async virtual Task<JsApiSignatureDto> GetAgentSignatureAsync(string url)
    {
        var jsApiTicket = await _ticketProvider.GetAgentTicketInfoAsync();
        var signatureData = _ticketProvider.GenerateSignature(jsApiTicket, HttpUtility.UrlDecode(url));

        return new JsApiSignatureDto(signatureData.Nonce, signatureData.Timestamp, signatureData.Signature);
    }

    public async virtual Task<JsApiSignatureDto> GetSignatureAsync(string url)
    {
        var jsApiTicket = await _ticketProvider.GetTicketInfoAsync();
        var signatureData = _ticketProvider.GenerateSignature(jsApiTicket, HttpUtility.UrlDecode(url));

        return new JsApiSignatureDto(signatureData.Nonce, signatureData.Timestamp, signatureData.Signature);
    }
}

using LINGYUN.Abp.WeChat.Work.Security;
using LINGYUN.Abp.WeChat.Work.Security.Models;
using LINGYUN.Abp.WeChat.Work.Settings;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.WeChat.Work.Message;
public class WeChatWorkMessageAppService : ApplicationService, IWeChatWorkMessageAppService
{
    private readonly IWeChatWorkCryptoService _cryptoService;
    private readonly WeChatWorkOptions _options;

    public WeChatWorkMessageAppService(
        IWeChatWorkCryptoService cryptoService,
        IOptionsMonitor<WeChatWorkOptions> options)
    {
        _cryptoService = cryptoService;
        _options = options.CurrentValue;
    }

    public async virtual Task<string> Handle(string agentId, MessageValidationInput input)
    {
        var corpId = await SettingProvider.GetOrNullAsync(WeChatWorkSettingNames.Connection.CorpId);
        Check.NotNullOrEmpty(corpId, nameof(corpId));

        var applicationConfiguration = _options.Applications.GetConfiguration(agentId);
        var cryptoConfiguration = applicationConfiguration.GetCryptoConfiguration("Message");
        var echoData = new WeChatWorkCryptoEchoData(
            input.EchoStr,
            corpId,
            cryptoConfiguration.Token,
            cryptoConfiguration.EncodingAESKey,
            input.Msg_Signature,
            input.TimeStamp.ToString(),
            input.Nonce);

        var echoStr = _cryptoService.Validation(echoData);

        return echoStr;
    }

    public async virtual Task<string> Handle(string agentId, MessageHandleInput input)
    {
        var corpId = await SettingProvider.GetOrNullAsync(WeChatWorkSettingNames.Connection.CorpId);
        Check.NotNullOrEmpty(corpId, nameof(corpId));

        var applicationConfiguration = _options.Applications.GetConfiguration(agentId);
        var cryptoConfiguration = applicationConfiguration.GetCryptoConfiguration("Message");
        var decryptData = new WeChatWorkCryptoDecryptData(
            input.Data,
            corpId,
            cryptoConfiguration.Token,
            cryptoConfiguration.EncodingAESKey,
            input.Msg_Signature,
            input.TimeStamp.ToString(),
            input.Nonce);

        var msg = _cryptoService.Decrypt(decryptData);
        return msg;
    }
}

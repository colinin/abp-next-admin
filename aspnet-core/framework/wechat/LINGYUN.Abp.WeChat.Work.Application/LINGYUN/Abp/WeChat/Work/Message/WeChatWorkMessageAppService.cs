using LINGYUN.Abp.WeChat.Common.Crypto;
using LINGYUN.Abp.WeChat.Common.Crypto.Models;
using LINGYUN.Abp.WeChat.Common.Messages;
using LINGYUN.Abp.WeChat.Work.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.EventBus.Distributed;

namespace LINGYUN.Abp.WeChat.Work.Message;
public class WeChatWorkMessageAppService : ApplicationService, IWeChatWorkMessageAppService
{
    private readonly IWeChatCryptoService _cryptoService;
    private readonly WeChatWorkOptions _options;
    private readonly IDistributedEventBus _distributedEventBus;
    private readonly IMessageResolver _messageResolver;
    public WeChatWorkMessageAppService(
        IMessageResolver messageResolver,
        IWeChatCryptoService cryptoService,
        IDistributedEventBus distributedEventBus,
        IOptionsMonitor<WeChatWorkOptions> options)
    {
        _cryptoService = cryptoService;
        _messageResolver = messageResolver;
        _distributedEventBus = distributedEventBus;
        _options = options.CurrentValue;
    }

    public async virtual Task<string> Handle(string agentId, MessageValidationInput input)
    {
        var corpId = await SettingProvider.GetOrNullAsync(WeChatWorkSettingNames.Connection.CorpId);
        Check.NotNullOrEmpty(corpId, nameof(corpId));

        var applicationConfiguration = _options.Applications.GetConfiguration(agentId);
        var cryptoConfiguration = applicationConfiguration.GetCryptoConfiguration("Message");
        var echoData = new WeChatCryptoEchoData(
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

        var messageData = new MessageResolveData(
            corpId,
            cryptoConfiguration.Token,
            cryptoConfiguration.EncodingAESKey,
            input.Msg_Signature,
            input.TimeStamp,
            input.Nonce,
            input.Data);

        var result = await _messageResolver.ResolveMessageAsync(messageData);
        if (result.Message == null)
        {
            Logger.LogWarning(input.Data);
            Logger.LogWarning("解析消息失败, 无法处理企业微信消息.");
        }
        else
        {
            Logger.LogInformation(result.Message.SerializeToXml());
            var eto = result.Message.ToEto();
            await _distributedEventBus.PublishAsync(eto.GetType(), eto);
        }
        // https://developer.work.weixin.qq.com/document/path/90238
        return "success";
    }
}

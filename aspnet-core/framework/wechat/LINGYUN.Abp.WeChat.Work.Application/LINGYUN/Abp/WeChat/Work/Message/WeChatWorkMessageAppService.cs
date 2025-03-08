using LINGYUN.Abp.WeChat.Common.Crypto;
using LINGYUN.Abp.WeChat.Common.Crypto.Models;
using LINGYUN.Abp.WeChat.Common.Messages;
using LINGYUN.Abp.WeChat.Work.Settings;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.EventBus.Distributed;

namespace LINGYUN.Abp.WeChat.Work.Message;
public class WeChatWorkMessageAppService : ApplicationService, IWeChatWorkMessageAppService
{
    private readonly IWeChatCryptoService _cryptoService;
    private readonly IDistributedEventBus _distributedEventBus;
    private readonly IMessageResolver _messageResolver;
    public WeChatWorkMessageAppService(
        IMessageResolver messageResolver,
        IWeChatCryptoService cryptoService,
        IDistributedEventBus distributedEventBus)
    {
        _cryptoService = cryptoService;
        _messageResolver = messageResolver;
        _distributedEventBus = distributedEventBus;
    }

    public async virtual Task<string> Handle(MessageValidationInput input)
    {
        var allSettings = await SettingProvider.GetAllAsync(
            new[] { 
                WeChatWorkSettingNames.Connection.CorpId, 
                WeChatWorkSettingNames.Connection.Token, 
                WeChatWorkSettingNames.Connection.EncodingAESKey,
            } );

        var corpId = allSettings.FirstOrDefault(x => x.Name == WeChatWorkSettingNames.Connection.CorpId)?.Value;
        var token = allSettings.FirstOrDefault(x => x.Name == WeChatWorkSettingNames.Connection.Token)?.Value;
        var aesKey = allSettings.FirstOrDefault(x => x.Name == WeChatWorkSettingNames.Connection.EncodingAESKey)?.Value;

        Check.NotNullOrEmpty(corpId, nameof(corpId));
        Check.NotNullOrEmpty(token, nameof(token));
        Check.NotNullOrEmpty(aesKey, nameof(aesKey));

        var echoData = new WeChatCryptoEchoData(
            input.EchoStr,
            corpId,
            token,
            aesKey,
            input.Msg_Signature,
            input.TimeStamp.ToString(),
            input.Nonce);

        var echoStr = _cryptoService.Validation(echoData);

        return echoStr;
    }

    public async virtual Task<string> Handle(MessageHandleInput input)
    {
        var allSettings = await SettingProvider.GetAllAsync(
            new[] {
                WeChatWorkSettingNames.Connection.CorpId,
                WeChatWorkSettingNames.Connection.Token,
                WeChatWorkSettingNames.Connection.EncodingAESKey,
            });

        var corpId = allSettings.FirstOrDefault(x => x.Name == WeChatWorkSettingNames.Connection.CorpId)?.Value;
        var token = allSettings.FirstOrDefault(x => x.Name == WeChatWorkSettingNames.Connection.Token)?.Value;
        var aesKey = allSettings.FirstOrDefault(x => x.Name == WeChatWorkSettingNames.Connection.EncodingAESKey)?.Value;

        Check.NotNullOrEmpty(corpId, nameof(corpId));
        Check.NotNullOrEmpty(token, nameof(token));
        Check.NotNullOrEmpty(aesKey, nameof(aesKey));

        var messageData = new MessageResolveData(
            corpId,
            token,
            aesKey,
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

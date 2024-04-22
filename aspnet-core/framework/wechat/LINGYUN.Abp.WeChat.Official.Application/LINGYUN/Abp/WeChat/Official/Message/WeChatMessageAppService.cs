using LINGYUN.Abp.WeChat.Common.Crypto;
using LINGYUN.Abp.WeChat.Common.Crypto.Models;
using LINGYUN.Abp.WeChat.Common.Messages;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.EventBus.Distributed;

namespace LINGYUN.Abp.WeChat.Official.Message;
public class WeChatMessageAppService : ApplicationService, IWeChatMessageAppService
{
    private readonly IWeChatCryptoService _cryptoService;
    private readonly AbpWeChatOfficialOptionsFactory _optionsFactory;
    private readonly IDistributedEventBus _distributedEventBus;
    private readonly IMessageResolver _messageResolver;
    public WeChatMessageAppService(
        IMessageResolver messageResolver,
        IWeChatCryptoService cryptoService,
        IDistributedEventBus distributedEventBus,
        AbpWeChatOfficialOptionsFactory optionsFactory)
    {
        _cryptoService = cryptoService;
        _optionsFactory = optionsFactory;
        _messageResolver = messageResolver;
        _distributedEventBus = distributedEventBus;
    }

    public async virtual Task<string> Handle(MessageValidationInput input)
    {
        var options = await _optionsFactory.CreateAsync();

        Check.NotNull(options, nameof(options));

        // 沙盒测试时，无需验证消息
        if (options.IsSandBox)
        {
            return input.EchoStr;
        }

        var echoData = new WeChatCryptoEchoData(
            input.EchoStr,
            options.AppId,
            options.Token,
            options.EncodingAESKey,
            input.Signature,
            input.TimeStamp.ToString(),
            input.Nonce);

        var echoStr = _cryptoService.Validation(echoData);

        return echoStr;
    }

    public async virtual Task<string> Handle(MessageHandleInput input)
    {
        var options = await _optionsFactory.CreateAsync();

        Check.NotNull(options, nameof(options));

        var messageData = new MessageResolveData(
            options.AppId,
            options.Token,
            options.EncodingAESKey,
            input.Signature,
            input.TimeStamp,
            input.Nonce,
            input.Data);

        var result = await _messageResolver.ResolveMessageAsync(messageData);
        if (result.Message == null)
        {
            Logger.LogWarning(input.Data);
            Logger.LogWarning("解析消息失败, 无法处理微信消息.");
        }
        else
        {
            Logger.LogInformation(result.Message.SerializeToXml());
            var eto = result.Message.ToEto();
            await _distributedEventBus.PublishAsync(eto.GetType(), eto);
        }
        // https://developers.weixin.qq.com/doc/offiaccount/Message_Management/Passive_user_reply_message.html
        return "success";
    }
}

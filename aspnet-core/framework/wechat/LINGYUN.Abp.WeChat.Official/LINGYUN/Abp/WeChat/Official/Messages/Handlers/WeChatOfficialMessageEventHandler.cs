using LINGYUN.Abp.WeChat.Common.Messages;
using LINGYUN.Abp.WeChat.Common.Messages.Handlers;
using LINGYUN.Abp.WeChat.Official.Messages.Models;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace LINGYUN.Abp.WeChat.Official.Messages.Handlers;
public class WeChatOfficialMessageEventHandler :
    IDistributedEventHandler<WeChatOfficialGeneralMessageEto<TextMessage>>,
    IDistributedEventHandler<WeChatOfficialGeneralMessageEto<LinkMessage>>,
    IDistributedEventHandler<WeChatOfficialGeneralMessageEto<VoiceMessage>>,
    IDistributedEventHandler<WeChatOfficialGeneralMessageEto<VideoMessage>>,
    IDistributedEventHandler<WeChatOfficialGeneralMessageEto<PictureMessage>>,
    IDistributedEventHandler<WeChatOfficialGeneralMessageEto<GeoLocationMessage>>,
    ITransientDependency
{
    private readonly IMessageHandler _messageHandler;

    public WeChatOfficialMessageEventHandler(IMessageHandler messageHandler)
    {
        _messageHandler = messageHandler;
    }

    public async virtual Task HandleEventAsync(WeChatOfficialGeneralMessageEto<TextMessage> eventData)
    {
        await _messageHandler.HandleMessageAsync(eventData.Message);
    }

    public async virtual Task HandleEventAsync(WeChatOfficialGeneralMessageEto<LinkMessage> eventData)
    {
        await _messageHandler.HandleMessageAsync(eventData.Message);
    }

    public async virtual Task HandleEventAsync(WeChatOfficialGeneralMessageEto<VoiceMessage> eventData)
    {
        await _messageHandler.HandleMessageAsync(eventData.Message);
    }

    public async virtual Task HandleEventAsync(WeChatOfficialGeneralMessageEto<VideoMessage> eventData)
    {
        await _messageHandler.HandleMessageAsync(eventData.Message);
    }

    public async virtual Task HandleEventAsync(WeChatOfficialGeneralMessageEto<PictureMessage> eventData)
    {
        await _messageHandler.HandleMessageAsync(eventData.Message);
    }

    public async virtual Task HandleEventAsync(WeChatOfficialGeneralMessageEto<GeoLocationMessage> eventData)
    {
        await _messageHandler.HandleMessageAsync(eventData.Message);
    }
}

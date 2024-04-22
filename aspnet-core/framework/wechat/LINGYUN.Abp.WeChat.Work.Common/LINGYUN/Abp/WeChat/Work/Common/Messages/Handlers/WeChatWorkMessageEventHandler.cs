using LINGYUN.Abp.WeChat.Common.Messages.Handlers;
using LINGYUN.Abp.WeChat.Work.Common.Messages.Models;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace LINGYUN.Abp.WeChat.Work.Common.Messages.Handlers;
public class WeChatWorkMessageEventHandler :
    IDistributedEventHandler<WeChatWorkGeneralMessageEto<TextMessage>>,
    IDistributedEventHandler<WeChatWorkGeneralMessageEto<LinkMessage>>,
    IDistributedEventHandler<WeChatWorkGeneralMessageEto<VoiceMessage>>,
    IDistributedEventHandler<WeChatWorkGeneralMessageEto<VideoMessage>>,
    IDistributedEventHandler<WeChatWorkGeneralMessageEto<PictureMessage>>,
    IDistributedEventHandler<WeChatWorkGeneralMessageEto<GeoLocationMessage>>,
    ITransientDependency
{
    private readonly IMessageHandler _messageHandler;

    public WeChatWorkMessageEventHandler(IMessageHandler messageHandler)
    {
        _messageHandler = messageHandler;
    }

    public async virtual Task HandleEventAsync(WeChatWorkGeneralMessageEto<TextMessage> eventData)
    {
        await _messageHandler.HandleMessageAsync(eventData.Message);
    }

    public async virtual Task HandleEventAsync(WeChatWorkGeneralMessageEto<LinkMessage> eventData)
    {
        await _messageHandler.HandleMessageAsync(eventData.Message);
    }

    public async virtual Task HandleEventAsync(WeChatWorkGeneralMessageEto<VoiceMessage> eventData)
    {
        await _messageHandler.HandleMessageAsync(eventData.Message);
    }

    public async virtual Task HandleEventAsync(WeChatWorkGeneralMessageEto<VideoMessage> eventData)
    {
        await _messageHandler.HandleMessageAsync(eventData.Message);
    }

    public async virtual Task HandleEventAsync(WeChatWorkGeneralMessageEto<PictureMessage> eventData)
    {
        await _messageHandler.HandleMessageAsync(eventData.Message);
    }

    public async virtual Task HandleEventAsync(WeChatWorkGeneralMessageEto<GeoLocationMessage> eventData)
    {
        await _messageHandler.HandleMessageAsync(eventData.Message);
    }
}

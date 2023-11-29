using LINGYUN.Abp.WeChat.Common.Messages;
using LINGYUN.Abp.WeChat.Common.Messages.Handlers;
using LINGYUN.Abp.WeChat.Official.Messages.Models;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace LINGYUN.Abp.WeChat.Official.Messages.Handlers;
public class WeChatOfficialEventEventHandler :
    IDistributedEventHandler<WeChatOfficialEventMessageEto<CustomMenuEvent>>,
    IDistributedEventHandler<WeChatOfficialEventMessageEto<UserSubscribeEvent>>,
    IDistributedEventHandler<WeChatOfficialEventMessageEto<UserUnSubscribeEvent>>,
    IDistributedEventHandler<WeChatOfficialEventMessageEto<ParametricQrCodeEvent>>,
    IDistributedEventHandler<WeChatOfficialEventMessageEto<MenuClickJumpLinkEvent>>,
    IDistributedEventHandler<WeChatOfficialEventMessageEto<ReportingGeoLocationEvent>>,
    ITransientDependency
{
    private readonly IMessageHandler _messageHandler;

    public WeChatOfficialEventEventHandler(IMessageHandler messageHandler)
    {
        _messageHandler = messageHandler;
    }

    public async virtual Task HandleEventAsync(WeChatOfficialEventMessageEto<CustomMenuEvent> eventData)
    {
        await _messageHandler.HandleEventAsync(eventData.Event);
    }

    public async virtual Task HandleEventAsync(WeChatOfficialEventMessageEto<UserSubscribeEvent> eventData)
    {
        await _messageHandler.HandleEventAsync(eventData.Event);
    }

    public async virtual Task HandleEventAsync(WeChatOfficialEventMessageEto<UserUnSubscribeEvent> eventData)
    {
        await _messageHandler.HandleEventAsync(eventData.Event);
    }

    public async virtual Task HandleEventAsync(WeChatOfficialEventMessageEto<ParametricQrCodeEvent> eventData)
    {
        await _messageHandler.HandleEventAsync(eventData.Event);
    }

    public async virtual Task HandleEventAsync(WeChatOfficialEventMessageEto<MenuClickJumpLinkEvent> eventData)
    {
        await _messageHandler.HandleEventAsync(eventData.Event);
    }

    public async virtual Task HandleEventAsync(WeChatOfficialEventMessageEto<ReportingGeoLocationEvent> eventData)
    {
        await _messageHandler.HandleEventAsync(eventData.Event);
    }
}

using LINGYUN.Abp.WeChat.Common.Messages.Handlers;
using LINGYUN.Abp.WeChat.Work.Common.Messages.Models;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace LINGYUN.Abp.WeChat.Work.Common.Messages.Handlers;
public class WeChatWorkEventEventHandler :
    IDistributedEventHandler<WeChatWorkEventMessageEto<CustomMenuPushEvent>>,
    IDistributedEventHandler<WeChatWorkEventMessageEto<MenuClickJumpLinkPushEvent>>,
    IDistributedEventHandler<WeChatWorkEventMessageEto<ScanCodePushEvent>>,
    IDistributedEventHandler<WeChatWorkEventMessageEto<ScanCodeWaitMsgPushEvent>>,
    IDistributedEventHandler<WeChatWorkEventMessageEto<PictureSystemPushEvent>>,
    IDistributedEventHandler<WeChatWorkEventMessageEto<PictureAlbumPushEvent>>,
    IDistributedEventHandler<WeChatWorkEventMessageEto<PictureWeixinPushEvent>>,
    IDistributedEventHandler<WeChatWorkEventMessageEto<UserSubscribeEvent>>,
    IDistributedEventHandler<WeChatWorkEventMessageEto<UserUnSubscribeEvent>>,
    IDistributedEventHandler<WeChatWorkEventMessageEto<EnterAgentEvent>>,
    IDistributedEventHandler<WeChatWorkEventMessageEto<ReportingGeoLocationEvent>>,
    IDistributedEventHandler<WeChatWorkEventMessageEto<GeoLocationSelectPushEevent>>,
    IDistributedEventHandler<WeChatWorkEventMessageEto<BatchJobResultEvent>>,
    IDistributedEventHandler<WeChatWorkEventMessageEto<ApprovalStatusChangeEvent>>,
    IDistributedEventHandler<WeChatWorkEventMessageEto<ShareAgentChangeEvent>>,
    IDistributedEventHandler<WeChatWorkEventMessageEto<ShareChainChangeEvent>>,
    IDistributedEventHandler<WeChatWorkEventMessageEto<TemplateCardPushEvent>>,
    IDistributedEventHandler<WeChatWorkEventMessageEto<TemplateCardMenuPushEvent>>,
    IDistributedEventHandler<WeChatWorkEventMessageEto<CloseInActiveAgentEvent>>,
    IDistributedEventHandler<WeChatWorkEventMessageEto<ReOpenInActiveAgentEevent>>,
    IDistributedEventHandler<WeChatWorkEventMessageEto<CreateUserEvent>>,
    IDistributedEventHandler<WeChatWorkEventMessageEto<UpdateUserEvent>>,
    IDistributedEventHandler<WeChatWorkEventMessageEto<DeleteUserEvent>>,
    IDistributedEventHandler<WeChatWorkEventMessageEto<CreateDepartmentEvent>>,
    IDistributedEventHandler<WeChatWorkEventMessageEto<UpdateDepartmentEvent>>,
    IDistributedEventHandler<WeChatWorkEventMessageEto<DeleteDepartmentEvent>>,
    IDistributedEventHandler<WeChatWorkEventMessageEto<UserTagChangeEvent>>,
    ITransientDependency
{
    private readonly IMessageHandler _messageHandler;

    public WeChatWorkEventEventHandler(IMessageHandler messageHandler)
    {
        _messageHandler = messageHandler;
    }

    public async virtual Task HandleEventAsync(WeChatWorkEventMessageEto<UserSubscribeEvent> eventData)
    {
        await _messageHandler.HandleEventAsync(eventData.Event);
    }

    public async virtual Task HandleEventAsync(WeChatWorkEventMessageEto<UserUnSubscribeEvent> eventData)
    {
        await _messageHandler.HandleEventAsync(eventData.Event);
    }

    public async virtual Task HandleEventAsync(WeChatWorkEventMessageEto<ReportingGeoLocationEvent> eventData)
    {
        await _messageHandler.HandleEventAsync(eventData.Event);
    }

    public async virtual Task HandleEventAsync(WeChatWorkEventMessageEto<CustomMenuPushEvent> eventData)
    {
        await _messageHandler.HandleEventAsync(eventData.Event);
    }

    public async virtual Task HandleEventAsync(WeChatWorkEventMessageEto<MenuClickJumpLinkPushEvent> eventData)
    {
        await _messageHandler.HandleEventAsync(eventData.Event);
    }

    public async virtual Task HandleEventAsync(WeChatWorkEventMessageEto<ScanCodePushEvent> eventData)
    {
        await _messageHandler.HandleEventAsync(eventData.Event);
    }

    public async virtual Task HandleEventAsync(WeChatWorkEventMessageEto<ScanCodeWaitMsgPushEvent> eventData)
    {
        await _messageHandler.HandleEventAsync(eventData.Event);
    }

    public async virtual Task HandleEventAsync(WeChatWorkEventMessageEto<PictureSystemPushEvent> eventData)
    {
        await _messageHandler.HandleEventAsync(eventData.Event);
    }

    public async virtual Task HandleEventAsync(WeChatWorkEventMessageEto<PictureAlbumPushEvent> eventData)
    {
        await _messageHandler.HandleEventAsync(eventData.Event);
    }

    public async virtual Task HandleEventAsync(WeChatWorkEventMessageEto<PictureWeixinPushEvent> eventData)
    {
        await _messageHandler.HandleEventAsync(eventData.Event);
    }

    public async virtual Task HandleEventAsync(WeChatWorkEventMessageEto<EnterAgentEvent> eventData)
    {
        await _messageHandler.HandleEventAsync(eventData.Event);
    }

    public async virtual Task HandleEventAsync(WeChatWorkEventMessageEto<GeoLocationSelectPushEevent> eventData)
    {
        await _messageHandler.HandleEventAsync(eventData.Event);
    }

    public async virtual Task HandleEventAsync(WeChatWorkEventMessageEto<BatchJobResultEvent> eventData)
    {
        await _messageHandler.HandleEventAsync(eventData.Event);
    }

    public async virtual Task HandleEventAsync(WeChatWorkEventMessageEto<ApprovalStatusChangeEvent> eventData)
    {
        await _messageHandler.HandleEventAsync(eventData.Event);
    }

    public async virtual Task HandleEventAsync(WeChatWorkEventMessageEto<ShareAgentChangeEvent> eventData)
    {
        await _messageHandler.HandleEventAsync(eventData.Event);
    }

    public async virtual Task HandleEventAsync(WeChatWorkEventMessageEto<ShareChainChangeEvent> eventData)
    {
        await _messageHandler.HandleEventAsync(eventData.Event);
    }

    public async virtual Task HandleEventAsync(WeChatWorkEventMessageEto<TemplateCardPushEvent> eventData)
    {
        await _messageHandler.HandleEventAsync(eventData.Event);
    }

    public async virtual Task HandleEventAsync(WeChatWorkEventMessageEto<TemplateCardMenuPushEvent> eventData)
    {
        await _messageHandler.HandleEventAsync(eventData.Event);
    }

    public async virtual Task HandleEventAsync(WeChatWorkEventMessageEto<CloseInActiveAgentEvent> eventData)
    {
        await _messageHandler.HandleEventAsync(eventData.Event);
    }

    public async virtual Task HandleEventAsync(WeChatWorkEventMessageEto<ReOpenInActiveAgentEevent> eventData)
    {
        await _messageHandler.HandleEventAsync(eventData.Event);
    }

    public async virtual Task HandleEventAsync(WeChatWorkEventMessageEto<CreateUserEvent> eventData)
    {
        await _messageHandler.HandleEventAsync(eventData.Event);
    }

    public async virtual Task HandleEventAsync(WeChatWorkEventMessageEto<UpdateUserEvent> eventData)
    {
        await _messageHandler.HandleEventAsync(eventData.Event);
    }

    public async virtual Task HandleEventAsync(WeChatWorkEventMessageEto<DeleteUserEvent> eventData)
    {
        await _messageHandler.HandleEventAsync(eventData.Event);
    }

    public async virtual Task HandleEventAsync(WeChatWorkEventMessageEto<CreateDepartmentEvent> eventData)
    {
        await _messageHandler.HandleEventAsync(eventData.Event);
    }

    public async virtual Task HandleEventAsync(WeChatWorkEventMessageEto<UpdateDepartmentEvent> eventData)
    {
        await _messageHandler.HandleEventAsync(eventData.Event);
    }

    public async virtual Task HandleEventAsync(WeChatWorkEventMessageEto<DeleteDepartmentEvent> eventData)
    {
        await _messageHandler.HandleEventAsync(eventData.Event);
    }

    public async virtual Task HandleEventAsync(WeChatWorkEventMessageEto<UserTagChangeEvent> eventData)
    {
        await _messageHandler.HandleEventAsync(eventData.Event);
    }
}

using LINGYUN.Abp.WeChat.Common;
using LINGYUN.Abp.WeChat.Common.Messages;
using LINGYUN.Abp.WeChat.Work.Common.Messages;
using LINGYUN.Abp.WeChat.Work.Common.Messages.Models;
using System.Collections.Generic;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.WeChat.Work.Common;

[DependsOn(
    typeof(AbpWeChatCommonModule))]
public class AbpWeChatWorkCommonModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpWeChatWorkMessageResolveOptions>(options =>
        {
            options.MapEvent("click", context => context.GetWeChatMessage<CustomMenuPushEvent>());
            options.MapEvent("view", context => context.GetWeChatMessage<MenuClickJumpLinkPushEvent>());
            options.MapEvent("scancode_push", context => context.GetWeChatMessage<ScanCodePushEvent>());
            options.MapEvent("scancode_waitmsg", context => context.GetWeChatMessage<ScanCodeWaitMsgPushEvent>());
            options.MapEvent("pic_sysphoto", context => context.GetWeChatMessage<PictureSystemPushEvent>());
            options.MapEvent("pic_photo_or_album", context => context.GetWeChatMessage<PictureAlbumPushEvent>());
            options.MapEvent("pic_weixin", context => context.GetWeChatMessage<PictureWeixinPushEvent>());
            options.MapEvent("subscribe", context => context.GetWeChatMessage<UserSubscribeEvent>());
            options.MapEvent("unsubscribe", context => context.GetWeChatMessage<UserUnSubscribeEvent>());
            options.MapEvent("enter_agent", context => context.GetWeChatMessage<EnterAgentEvent>());
            options.MapEvent("LOCATION", context => context.GetWeChatMessage<ReportingGeoLocationEvent>());
            options.MapEvent("location_select", context => context.GetWeChatMessage<GeoLocationSelectPushEevent>());
            options.MapEvent("batch_job_result", context => context.GetWeChatMessage<BatchJobResultEvent>());
            options.MapEvent("open_approval_change", context => context.GetWeChatMessage<ApprovalStatusChangeEvent>());
            options.MapEvent("share_agent_change", context => context.GetWeChatMessage<ShareAgentChangeEvent>());
            options.MapEvent("share_chain_change", context => context.GetWeChatMessage<ShareChainChangeEvent>());
            options.MapEvent("template_card_event", context => context.GetWeChatMessage<TemplateCardPushEvent>());
            options.MapEvent("template_card_menu_event", context => context.GetWeChatMessage<TemplateCardMenuPushEvent>());
            options.MapEvent("close_inactive_agent", context => context.GetWeChatMessage<CloseInActiveAgentEvent>());
            options.MapEvent("reopen_inactive_agent", context => context.GetWeChatMessage<ReOpenInActiveAgentEevent>());
            options.MapEvent("change_contact", context =>
            {
                //static UserChangeEvent CreateUserChangeEvent<TEvent>(string originXml) where TEvent : UserChangeEvent
                //{
                //    var events = new XmlDeserializationEvents();
                //    return originXml.DeserializeWeChatMessage<TEvent>(events);
                //}

                var changeType = context.GetMessageData("ChangeType");
                return changeType switch
                {
                    "create_user" => context.GetWeChatMessage<CreateUserEvent>(),
                    "update_user" => context.GetWeChatMessage<UpdateUserEvent>(),
                    "delete_user" => context.GetWeChatMessage<DeleteUserEvent>(),
                    "create_party" => context.GetWeChatMessage<CreateDepartmentEvent>(),
                    "update_party" => context.GetWeChatMessage<UpdateDepartmentEvent>(),
                    "delete_party" => context.GetWeChatMessage<DeleteDepartmentEvent>(),
                    "update_tag" => context.GetWeChatMessage<UserTagChangeEvent>(),
                    _ => throw new AbpWeChatException($"Contact change event {changeType} is not mounted!"),
                };
            });

            options.MapMessage("text", context => context.GetWeChatMessage<TextMessage>());
            options.MapMessage("image", context => context.GetWeChatMessage<PictureMessage>());
            options.MapMessage("voice", context => context.GetWeChatMessage<VoiceMessage>());
            options.MapMessage("video", context => context.GetWeChatMessage<VideoMessage>());
            options.MapMessage("shortvideo", context => context.GetWeChatMessage<VideoMessage>());
            options.MapMessage("location", context => context.GetWeChatMessage<GeoLocationMessage>());
            options.MapMessage("link", context => context.GetWeChatMessage<LinkMessage>());
        });

        Configure<AbpWeChatMessageResolveOptions>(options =>
        {
            // 事件处理器
            options.MessageResolvers.AddIfNotContains(new WeChatWorkEventResolveContributor());
            // 消息处理器
            options.MessageResolvers.AddIfNotContains(new WeChatWorkMessageResolveContributor());
        });
    }
}

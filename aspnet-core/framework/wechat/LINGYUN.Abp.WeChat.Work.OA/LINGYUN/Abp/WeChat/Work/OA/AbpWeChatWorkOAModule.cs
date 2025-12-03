using LINGYUN.Abp.WeChat.Common.Messages;
using LINGYUN.Abp.WeChat.Work.Common.Messages;
using LINGYUN.Abp.WeChat.Work.Localization;
using LINGYUN.Abp.WeChat.Work.OA.Messages.Models;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.WeChat.Work.OA;
/// <summary>
/// 企业微信办公模块
/// </summary>
[DependsOn(typeof(AbpWeChatWorkModule))]
public class AbpWeChatWorkOAModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpWeChatWorkMessageResolveOptions>(options =>
        {
            options.MapEvent("book_meeting_room", context => context.GetWeChatMessage<BookMeetingRoomEvent>());
            options.MapEvent("cancel_meeting_room", context => context.GetWeChatMessage<CancelMeetingRoomEvent>());

            options.MapEvent("delete_calendar", context => context.GetWeChatMessage<DeleteCalendarEvent>());
            options.MapEvent("modify_calendar", context => context.GetWeChatMessage<UpdateCalendarEvent>());
            options.MapEvent("delete_schedule", context => context.GetWeChatMessage<DeleteScheduleEvent>());
            options.MapEvent("modify_schedule", context => context.GetWeChatMessage<UpdateScheduleEvent>());
            options.MapEvent("respond_schedule", context => context.GetWeChatMessage<RespondScheduleEvent>());
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<WeChatWorkResource>()
                .AddVirtualJson("/LINGYUN/Abp/WeChat/Work/OA/Localization/Resources");
        });
    }
}

using LINGYUN.Abp.WeChat.Common.Messages;
using LINGYUN.Abp.WeChat.Common.Messages.Handlers;
using LINGYUN.Abp.WeChat.Localization;
using LINGYUN.Abp.WeChat.Official.Messages;
using LINGYUN.Abp.WeChat.Official.Messages.Handlers;
using LINGYUN.Abp.WeChat.Official.Messages.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.WeChat.Official
{
    [DependsOn(
        typeof(AbpWeChatModule))]
    public class AbpWeChatOfficialModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpWeChatOfficialMessageResolveOptions>(options =>
            {
                options.MapEvent("subscribe", context =>
                {
                    return context.HasMessageKey("Ticket")
                        // 用户未关注公众号时, 扫描带参数二维码后进行关注的事件
                        ? context.GetWeChatMessage<ParametricQrCodeEvent>()
                        // 用户关注/取消关注
                        : context.GetWeChatMessage<UserSubscribeEvent>();
                });
                options.MapEvent("unsubscribe", context => context.GetWeChatMessage<UserUnSubscribeEvent>());
                options.MapEvent("LOCATION", context => context.GetWeChatMessage<ReportingGeoLocationEvent>());
                options.MapEvent("CLICK", context => context.GetWeChatMessage<CustomMenuEvent>());
                options.MapEvent("VIEW", context => context.GetWeChatMessage<MenuClickJumpLinkEvent>());
                options.MapEvent("SCAN", context => context.GetWeChatMessage<ParametricQrCodeEvent>());

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
                options.MessageResolvers.AddIfNotContains(new WeChatOfficialEventResolveContributor());
                // 消息处理器
                options.MessageResolvers.AddIfNotContains(new WeChatOfficialMessageResolveContributor());
            });

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpWeChatOfficialModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<WeChatResource>()
                    .AddVirtualJson("/LINGYUN/Abp/WeChat/Official/Localization/Resources");
            });

            context.Services.AddAbpDynamicOptions<AbpWeChatOfficialOptions, AbpWeChatOfficialOptionsManager>();

            context.Services.AddHttpClient(AbpWeChatOfficialConsts.HttpClient,
                options =>
                {
                    options.BaseAddress = new Uri("https://mp.weixin.qq.com");
                });
        }
    }
}

using LINGYUN.Abp.WeChat.Common.Messages.Handlers;
using LINGYUN.Abp.WeChat.Official.Messages.Models;
using LINGYUN.Abp.WeChat.Official.Services;

namespace LY.MicroService.Applications.Single.WeChat.Official.Messages;
/// <summary>
/// 用户关注回复消息
/// </summary>
public class UserSubscribeEventContributor : IEventHandleContributor<UserSubscribeEvent>
{
    public async virtual Task HandleAsync(MessageHandleContext<UserSubscribeEvent> context)
    {
        var messageSender = context.ServiceProvider.GetRequiredService<IServiceCenterMessageSender>();

        await messageSender.SendAsync(
            new LINGYUN.Abp.WeChat.Official.Services.Models.TextMessageModel(
                context.Message.FromUserName,
                new LINGYUN.Abp.WeChat.Official.Services.Models.TextMessage(
                    "感谢您的关注, 点击菜单了解更多.")));
    }
}

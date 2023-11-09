using LINGYUN.Abp.WeChat.Common.Messages.Handlers;
using LINGYUN.Abp.WeChat.Official.Messages.Models;
using LINGYUN.Abp.WeChat.Official.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.Official.Messages.Handlers;
/// <summary>
/// 用户关注回复消息
/// </summary>
public class UserSubscribeEventContributor : IEventHandleContributor<UserSubscribeEvent>
{
    public async virtual Task HandleAsync(MessageHandleContext<UserSubscribeEvent> context)
    {
        var messageSender = context.ServiceProvider.GetRequiredService<IServiceCenterMessageSender>();

        await messageSender.SendAsync(
            new Services.Models.TextMessageModel(
                context.Message.FromUserName,
                new Services.Models.TextMessage(
                    "感谢您的关注, 点击菜单了解更多.")));
    }
}

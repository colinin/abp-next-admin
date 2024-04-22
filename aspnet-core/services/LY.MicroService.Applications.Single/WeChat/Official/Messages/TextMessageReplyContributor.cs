using LINGYUN.Abp.WeChat.Common.Messages.Handlers;
using LINGYUN.Abp.WeChat.Official.Messages.Models;
using LINGYUN.Abp.WeChat.Official.Services;

namespace LY.MicroService.Applications.Single.WeChat.Official.Messages;
/// <summary>
/// 文本消息客服回复
/// </summary>
public class TextMessageReplyContributor : IMessageHandleContributor<TextMessage>
{
    public async virtual Task HandleAsync(MessageHandleContext<TextMessage> context)
    {
        var messageSender = context.ServiceProvider.GetRequiredService<IServiceCenterMessageSender>();

        await messageSender.SendAsync(
            new LINGYUN.Abp.WeChat.Official.Services.Models.TextMessageModel(
                context.Message.FromUserName,
                new LINGYUN.Abp.WeChat.Official.Services.Models.TextMessage(
                    context.Message.Content)));
    }
}

using LINGYUN.Abp.WeChat.Common.Messages.Handlers;
using LINGYUN.Abp.WeChat.Work.Common.Messages.Models;
using LINGYUN.Abp.WeChat.Work.Messages;

namespace LY.MicroService.Applications.Single.WeChat.Work.Messages;
/// <summary>
/// 文本消息客服回复
/// </summary>
public class TextMessageReplyContributor : IMessageHandleContributor<TextMessage>
{
    public async virtual Task HandleAsync(MessageHandleContext<TextMessage> context)
    {
        var messageSender = context.ServiceProvider.GetRequiredService<IWeChatWorkMessageSender>();

        await messageSender.SendAsync(
            new LINGYUN.Abp.WeChat.Work.Messages.Models.WeChatWorkTextMessage(
                context.Message.AgentId.ToString(),
                new LINGYUN.Abp.WeChat.Work.Messages.Models.TextMessage(
                    context.Message.Content))
            {
                ToUser = context.Message.FromUserName,
            });
    }
}

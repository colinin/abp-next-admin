using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace LINGYUN.Abp.IM.Messages
{
    public abstract class MessageSenderBase : IMessageSender
    {
        protected IMessageStore Store { get; }
        protected ILogger Logger { get; }
        protected MessageSenderBase(
            IMessageStore store,
            ILogger logger)
        {
            Store = store;
            Logger = logger;
        }

        public virtual async Task SendMessageAsync(ChatMessage chatMessage)
        {
            // 持久化
            await Store.StoreMessageAsync(chatMessage);

            try
            {
                if (!chatMessage.GroupId.IsNullOrWhiteSpace())
                {
                    await SendMessageToGroupAsync(chatMessage);
                }
                else
                {
                    await SendMessageToUserAsync(chatMessage);
                }
            }
            catch (Exception ex)
            {
                Logger.LogWarning("Could not send message, group: {0}, formUser: {1}, toUser: {2}",
                    chatMessage.GroupId, chatMessage.FormUserName,
                    chatMessage.ToUserId.HasValue ? chatMessage.ToUserId.ToString() : "None");
                Logger.LogWarning("Send group message error: {0}", ex.Message);
            }
        }

        protected abstract Task SendMessageToGroupAsync(ChatMessage chatMessage);
        protected abstract Task SendMessageToUserAsync(ChatMessage chatMessage);
    }
}

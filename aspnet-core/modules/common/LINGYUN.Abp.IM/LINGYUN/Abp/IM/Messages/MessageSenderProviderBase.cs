using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.IM.Messages
{
    public abstract class MessageSenderProviderBase : IMessageSenderProvider, ITransientDependency
    {
        public abstract string Name { get; }

        protected IAbpLazyServiceProvider ServiceProvider { get; }

        protected ILoggerFactory LoggerFactory => ServiceProvider.LazyGetRequiredService<ILoggerFactory>();

        protected ILogger Logger => _lazyLogger.Value;
        private Lazy<ILogger> _lazyLogger => new Lazy<ILogger>(() => LoggerFactory?.CreateLogger(GetType().FullName) ?? NullLogger.Instance, true);

        protected MessageSenderProviderBase(IAbpLazyServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public virtual async Task SendMessageAsync(ChatMessage chatMessage)
        {
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

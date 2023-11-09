using LINGYUN.Abp.WeChat.Common.Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.WeChat.Common.Messages.Handlers;
public class MessageHandler : IMessageHandler, ITransientDependency
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly AbpWeChatMessageHandleOptions _handleOptions;

    public MessageHandler(
        IServiceScopeFactory serviceScopeFactory,
        IOptions<AbpWeChatMessageHandleOptions> handleOptions)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _handleOptions = handleOptions.Value;
    }

    public async virtual Task HandleEventAsync<TMessage>(TMessage data) where TMessage : WeChatEventMessage
    {
        if (_handleOptions.EventHandlers.TryGetValue(data.GetType(), out var handleTypes))
        {
            using var scope = _serviceScopeFactory.CreateScope();
            foreach (var handleType in handleTypes)
            {
                var handlerService = ActivatorUtilities.CreateInstance(scope.ServiceProvider, handleType);
                if (handlerService is IEventHandleContributor<TMessage> handler)
                {
                    var context = new MessageHandleContext<TMessage>(data, scope.ServiceProvider);
                    await handler.HandleAsync(context);
                }
            }
        }
    }

    public async virtual Task HandleMessageAsync<TMessage>(TMessage data) where TMessage : WeChatGeneralMessage
    {
        if (_handleOptions.MessageHandlers.TryGetValue(data.GetType(), out var handleTypes))
        {
            using var scope = _serviceScopeFactory.CreateScope();
            foreach (var handleType in handleTypes)
            {
                var handlerService = ActivatorUtilities.CreateInstance(scope.ServiceProvider, handleType);
                if (handlerService is IMessageHandleContributor<TMessage> handler)
                {
                    var context = new MessageHandleContext<TMessage>(data, scope.ServiceProvider);
                    await handler.HandleAsync(context);
                }
            }
        }
    }
}

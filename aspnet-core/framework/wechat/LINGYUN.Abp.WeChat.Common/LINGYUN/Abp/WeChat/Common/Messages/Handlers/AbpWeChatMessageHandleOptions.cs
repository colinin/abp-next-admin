using LINGYUN.Abp.WeChat.Common.Messages;
using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.WeChat.Common.Messages.Handlers;
public class AbpWeChatMessageHandleOptions
{
    internal IDictionary<Type, IList<Type>> EventHandlers { get; }
    internal IDictionary<Type, IList<Type>> MessageHandlers { get; }

    public AbpWeChatMessageHandleOptions()
    {
        EventHandlers = new Dictionary<Type, IList<Type>>();
        MessageHandlers = new Dictionary<Type, IList<Type>>();
    }

    public void MapEvent<TEvent, TEventHandler>()
        where TEvent : WeChatEventMessage
        where TEventHandler : IEventHandleContributor<TEvent>
    {
        var eventType = typeof(TEvent);
        if (!EventHandlers.ContainsKey(eventType))
        {
            EventHandlers.Add(eventType, new List<Type>());
        }
        EventHandlers[eventType].AddIfNotContains(typeof(TEventHandler));
    }

    public void MapMessage<TMessage, TMessageHandler>()
        where TMessage : WeChatGeneralMessage
        where TMessageHandler : IMessageHandleContributor<TMessage>
    {
        var eventType = typeof(TMessage);
        if (!MessageHandlers.ContainsKey(eventType))
        {
            MessageHandlers.Add(eventType, new List<Type>());
        }
        MessageHandlers[eventType].AddIfNotContains(typeof(TMessageHandler));
    }
}

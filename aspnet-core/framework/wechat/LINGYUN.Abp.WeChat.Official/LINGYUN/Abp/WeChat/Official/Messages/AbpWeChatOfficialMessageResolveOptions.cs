using LINGYUN.Abp.WeChat.Common.Messages;
using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.WeChat.Official.Messages;
public class AbpWeChatOfficialMessageResolveOptions
{
    public IDictionary<string, Func<IMessageResolveContext, WeChatEventMessage>> EventMaps { get; }
    public IDictionary<string, Func<IMessageResolveContext, WeChatGeneralMessage>> MessageMaps { get; }
    public AbpWeChatOfficialMessageResolveOptions()
    {
        EventMaps = new Dictionary<string, Func<IMessageResolveContext, WeChatEventMessage>>();
        MessageMaps = new Dictionary<string, Func<IMessageResolveContext, WeChatGeneralMessage>>();
    }

    public void MapEvent(string eventName, Func<IMessageResolveContext, WeChatEventMessage> mapFunc)
    {
        EventMaps[eventName] = mapFunc;
    }

    public void MapMessage(string messageType, Func<IMessageResolveContext, WeChatGeneralMessage> mapFunc)
    {
        MessageMaps[messageType] = mapFunc;
    }
}

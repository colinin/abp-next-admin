using LINGYUN.Abp.WeChat.Common.Messages;
using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.WeChat.Work.Common.Messages;
public class AbpWeChatWorkMessageResolveOptions
{
    public IDictionary<string, Func<IMessageResolveContext, WeChatWorkEventMessage>> EventMaps { get; }
    public IDictionary<string, Func<IMessageResolveContext, WeChatWorkGeneralMessage>> MessageMaps { get; }
    public AbpWeChatWorkMessageResolveOptions()
    {
        EventMaps = new Dictionary<string, Func<IMessageResolveContext, WeChatWorkEventMessage>>();
        MessageMaps = new Dictionary<string, Func<IMessageResolveContext, WeChatWorkGeneralMessage>>();
    }

    public void MapEvent(string eventName, Func<IMessageResolveContext, WeChatWorkEventMessage> mapFunc)
    {
        EventMaps[eventName] = mapFunc;
    }

    public void MapMessage(string messageType, Func<IMessageResolveContext, WeChatWorkGeneralMessage> mapFunc)
    {
        MessageMaps[messageType] = mapFunc;
    }
}

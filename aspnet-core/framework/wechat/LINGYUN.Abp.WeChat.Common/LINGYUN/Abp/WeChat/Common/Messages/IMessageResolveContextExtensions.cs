using System;

namespace LINGYUN.Abp.WeChat.Common.Messages;
public static class IMessageResolveContextExtensions
{
    public static bool HasMessageKey(this IMessageResolveContext context, string key)
    {
        return context.MessageData.Root.Element(key) != null;
    }

    public static string GetMessageData(this IMessageResolveContext context, string key)
    {
        return context.MessageData.Root.Element(key)?.Value;
    }

    public static T GetWeChatMessage<T>(this IMessageResolveContext context) where T : WeChatMessage
    {
        return context.Origin.DeserializeWeChatMessage<T>();
    }
}

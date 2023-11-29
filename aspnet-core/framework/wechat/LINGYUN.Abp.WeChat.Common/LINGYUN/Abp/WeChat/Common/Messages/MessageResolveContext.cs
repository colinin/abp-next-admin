using System;
using System.Xml.Linq;

namespace LINGYUN.Abp.WeChat.Common.Messages;
public class MessageResolveContext : IMessageResolveContext
{
    public IServiceProvider ServiceProvider { get; }
    public string Origin { get; }
    public XDocument MessageData { get; }
    public bool Handled { get; set; }
    public WeChatMessage Message { get; set; }

    public bool HasResolvedMessage()
    {
        return Handled || Message != null;
    }

    public MessageResolveContext(
        string origin,
        XDocument messageData,
        IServiceProvider serviceProvider)
    {
        Origin = origin;
        MessageData = messageData;
        ServiceProvider = serviceProvider;
    }
}

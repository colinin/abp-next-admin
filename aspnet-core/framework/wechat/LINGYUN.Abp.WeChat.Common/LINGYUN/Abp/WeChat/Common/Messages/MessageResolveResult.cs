using System.Collections.Generic;

namespace LINGYUN.Abp.WeChat.Common.Messages;
public class MessageResolveResult
{
    public string Input { get; internal set; }

    public WeChatMessage Message { get; set; }

    public List<string> AppliedResolvers { get; }

    public MessageResolveResult(string input)
    {
        Input = input;
        AppliedResolvers = new List<string>();
    }
}

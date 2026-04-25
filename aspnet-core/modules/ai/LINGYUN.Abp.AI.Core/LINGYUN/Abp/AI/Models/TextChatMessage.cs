using Microsoft.Extensions.AI;
using System;

namespace LINGYUN.Abp.AI.Models;
public class TextChatMessage : ChatMessage
{
    /// <summary>
    /// 消息内容
    /// </summary>
    public string Content { get; }
    public TextChatMessage(
        string workspace,
        string content,
        ChatRole? role = null,
        DateTime? createdAt = null)
        : base(workspace, role, createdAt)
    {
        Content = content;
    }

    public override string GetMessagePrompt()
    {
        return Content;
    }
}

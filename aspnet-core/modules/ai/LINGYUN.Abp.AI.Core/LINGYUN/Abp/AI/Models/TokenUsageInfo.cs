using System;
using System.Text;

namespace LINGYUN.Abp.AI.Models;
public class TokenUsageInfo
{
    public string Workspace { get; }
    public Guid? MessageId { get; private set; }
    public Guid ConversationId { get; private set; }
    public long? InputTokenCount { get; set; }
    public long? OutputTokenCount { get; set; }
    public long? TotalTokenCount { get; set; }
    public long? CachedInputTokenCount { get; set; }
    public long? ReasoningTokenCount { get; set; }
    public TokenUsageInfo(string workspace, Guid conversationId)
    {
        Workspace = workspace;
        ConversationId = conversationId;
    }
    public virtual TokenUsageInfo WithMessageId(Guid id)
    {
        MessageId = id;
        return this;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.AppendLine("---------------------- TokenUsage Begin ----------------------");
        sb.AppendLine($"====== Workspace           - {Workspace}");
        sb.AppendLine($"====== MessageId           - {MessageId}");
        sb.AppendLine($"====== ConversationId      - {ConversationId}");
        sb.AppendLine($"====== InputTokenCount     - {InputTokenCount}");
        sb.AppendLine($"====== OutputTokenCount    - {OutputTokenCount}");
        sb.AppendLine($"====== TotalTokenCount     - {TotalTokenCount}");
        sb.AppendLine($"====== ReasoningTokenCount - {ReasoningTokenCount}");
        sb.AppendLine("---------------------- TokenUsage End ----------------------");

        return sb.ToString();
    }
}

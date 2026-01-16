namespace LINGYUN.Abp.AI.Models;
public class TokenUsageInfo
{
    public string Workspace { get; }
    public long? InputTokenCount { get; set; }
    public long? OutputTokenCount { get; set; }
    public long? TotalTokenCount { get; set; }
    public long? CachedInputTokenCount { get; set; }
    public long? ReasoningTokenCount { get; set; }
    public TokenUsageInfo(string workspace)
    {
        Workspace = workspace;
    }
}

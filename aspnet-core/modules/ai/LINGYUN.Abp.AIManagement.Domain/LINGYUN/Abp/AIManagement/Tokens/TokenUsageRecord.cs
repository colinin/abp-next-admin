using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.AIManagement.Tokens;
public class TokenUsageRecord : AuditedEntity<Guid>, IMultiTenant
{
    public Guid? TenantId { get; private set; }
    public Guid? MessageId { get; private set; }
    public Guid? ConversationId { get; private set; }
    public long? InputTokenCount { get; set; }
    public long? OutputTokenCount { get; set; }
    public long? TotalTokenCount { get; set; }
    public long? CachedInputTokenCount { get; set; }
    public long? ReasoningTokenCount { get; set; }
    protected TokenUsageRecord()
    {

    }

    public TokenUsageRecord(
        Guid id,
        Guid? messageId = null,
        Guid? conversationId = null, 
        long? inputTokenCount = null,
        long? outputTokenCount = null,
        long? totalTokenCount = null,
        long? cachedInputTokenCount = null,
        long? reasoningTokenCount = null,
        Guid? tenantId = null)
        : base(id)
    {
        MessageId = messageId;
        ConversationId = conversationId;
        InputTokenCount = inputTokenCount;
        OutputTokenCount = outputTokenCount;
        TotalTokenCount = totalTokenCount;
        CachedInputTokenCount = cachedInputTokenCount;
        ReasoningTokenCount = reasoningTokenCount;
        TenantId = tenantId;
    }
}

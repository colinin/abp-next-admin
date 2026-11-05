using System;

namespace LINGYUN.Abp.OperationRateLimiting;

/// <summary>
/// Creates the store request of a rule check. The algorithm specific values are provided by
/// the rule implementation (through the extension methods of its algorithm namespace).
/// </summary>
public static class OperationRateLimitingStoreRequestFactory
{
    public static OperationRateLimitingStoreRequest Create(
        OperationRateLimitingRuleContext context,
        TimeSpan duration,
        int maxCount,
        int segments = 10,
        int tokensPerPeriod = 1)
    {
        return new OperationRateLimitingStoreRequest
        {
            Key = context.Key,
            Algorithm = context.Rule.Algorithm,
            Duration = duration,
            MaxCount = maxCount,
            Segments = segments,
            TokensPerPeriod = tokensPerPeriod,
            Consume = context.Consume,
            Now = context.Now,
            Properties = context.Rule.Properties
        };
    }
}

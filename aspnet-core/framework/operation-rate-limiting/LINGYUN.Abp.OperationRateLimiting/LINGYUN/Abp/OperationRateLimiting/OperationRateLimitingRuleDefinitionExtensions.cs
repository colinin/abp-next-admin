using LINGYUN.Abp.OperationRateLimiting.Algorithms.Concurrency;
using LINGYUN.Abp.OperationRateLimiting.Algorithms.FixedWindow;
using LINGYUN.Abp.OperationRateLimiting.Algorithms.SlidingWindow;
using LINGYUN.Abp.OperationRateLimiting.Algorithms.TokenBucket;

namespace LINGYUN.Abp.OperationRateLimiting;

public static class OperationRateLimitingRuleDefinitionExtensions
{
    /// <summary>
    /// Returns the counter key descriptor of the rule. The algorithm specific descriptor is
    /// implemented by the extension method of the corresponding algorithm namespace.
    /// </summary>
    public static string GetRuleKeyDescriptor(this OperationRateLimitingRuleDefinition definition)
    {
        return definition.Algorithm switch
        {
            OperationRateLimitingAlgorithms.FixedWindow =>
                OperationRateLimitingRuleDefinitionFixedWindowExtensions.GetKeyDescriptor(definition),
            OperationRateLimitingAlgorithms.SlidingWindow =>
                OperationRateLimitingRuleDefinitionSlidingWindowExtensions.GetKeyDescriptor(definition),
            OperationRateLimitingAlgorithms.TokenBucket =>
                OperationRateLimitingRuleDefinitionTokenBucketExtensions.GetKeyDescriptor(definition),
            OperationRateLimitingAlgorithms.Concurrency =>
                OperationRateLimitingRuleDefinitionConcurrencyExtensions.GetKeyDescriptor(definition),
            _ => OperationRateLimitingRuleDefinitionValueHelper.BuildKeyDescriptor(definition)
        };
    }
}

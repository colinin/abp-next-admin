namespace LINGYUN.Abp.OperationRateLimiting.Algorithms.Concurrency;

/// <summary>
/// Concurrency properties of <see cref="OperationRateLimitingRuleDefinition"/>.
/// </summary>
/// <remarks>
/// Concurrency only uses the common <see cref="OperationRateLimitingRuleDefinition.Duration"/>
/// (lease duration) and <see cref="OperationRateLimitingRuleDefinition.MaxCount"/> (permit limit).
/// </remarks>
public static class OperationRateLimitingRuleDefinitionConcurrencyExtensions
{
    public static string GetKeyDescriptor(this OperationRateLimitingRuleDefinition definition)
    {
        return OperationRateLimitingRuleDefinitionValueHelper.BuildKeyDescriptor(definition);
    }
}

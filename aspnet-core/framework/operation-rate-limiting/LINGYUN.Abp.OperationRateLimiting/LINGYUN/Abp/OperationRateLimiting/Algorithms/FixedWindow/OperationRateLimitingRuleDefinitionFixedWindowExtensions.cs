namespace LINGYUN.Abp.OperationRateLimiting.Algorithms.FixedWindow;

/// <summary>
/// Fixed window properties of <see cref="OperationRateLimitingRuleDefinition"/>.
/// </summary>
/// <remarks>
/// Fixed window only uses the common <see cref="OperationRateLimitingRuleDefinition.Duration"/>
/// and <see cref="OperationRateLimitingRuleDefinition.MaxCount"/> properties.
/// </remarks>
public static class OperationRateLimitingRuleDefinitionFixedWindowExtensions
{
    public static string GetKeyDescriptor(this OperationRateLimitingRuleDefinition definition)
    {
        return OperationRateLimitingRuleDefinitionValueHelper.BuildKeyDescriptor(definition);
    }
}

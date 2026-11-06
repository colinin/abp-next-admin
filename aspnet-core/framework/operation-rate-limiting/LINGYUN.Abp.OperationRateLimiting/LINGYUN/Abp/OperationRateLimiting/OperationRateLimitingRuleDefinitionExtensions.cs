namespace LINGYUN.Abp.OperationRateLimiting;

public static class OperationRateLimitingRuleDefinitionExtensions
{
    /// <summary>
    /// Returns the counter key descriptor of the rule.
    /// </summary>
    /// <remarks>
    /// Every algorithm uses the same descriptor format (name or algorithm/parameters), so the
    /// built-in algorithm does not need to be switched on here. Custom algorithms use the same
    /// fallback format.
    /// </remarks>
    public static string GetRuleKeyDescriptor(this OperationRateLimitingRuleDefinition definition)
    {
        return OperationRateLimitingRuleDefinitionValueHelper.BuildKeyDescriptor(definition);
    }
}

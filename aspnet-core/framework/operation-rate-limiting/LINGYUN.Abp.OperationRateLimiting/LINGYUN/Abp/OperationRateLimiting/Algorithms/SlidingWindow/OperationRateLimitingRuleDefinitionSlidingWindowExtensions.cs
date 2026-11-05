using Volo.Abp;

namespace LINGYUN.Abp.OperationRateLimiting.Algorithms.SlidingWindow;

/// <summary>
/// Sliding window properties of <see cref="OperationRateLimitingRuleDefinition"/>.
/// </summary>
public static class OperationRateLimitingRuleDefinitionSlidingWindowExtensions
{
    private const string SegmentsProperty = "SlidingWindow:Segments";

    public static int GetSegments(this OperationRateLimitingRuleDefinition definition)
    {
        return OperationRateLimitingRuleDefinitionValueHelper.GetInt(definition, SegmentsProperty, 10);
    }

    public static void SetSegments(this OperationRateLimitingRuleDefinition definition, int segments)
    {
        definition.WithProperty(
            SegmentsProperty,
            OperationRateLimitingRuleDefinitionValueHelper.FormatInt(segments));
    }

    public static string GetKeyDescriptor(this OperationRateLimitingRuleDefinition definition)
    {
        return OperationRateLimitingRuleDefinitionValueHelper.BuildKeyDescriptor(definition);
    }

    public static void Validate(this OperationRateLimitingRuleDefinition definition, string policyName)
    {
        if (definition.GetSegments() < 2)
        {
            throw new AbpException($"The number of segments of a sliding window rule in operation rate limiting policy '{policyName}' must be at least 2.");
        }
    }
}

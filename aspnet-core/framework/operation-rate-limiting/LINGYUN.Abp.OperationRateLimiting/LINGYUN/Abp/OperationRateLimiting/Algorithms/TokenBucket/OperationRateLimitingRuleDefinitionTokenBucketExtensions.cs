using Volo.Abp;

namespace LINGYUN.Abp.OperationRateLimiting.Algorithms.TokenBucket;

/// <summary>
/// Token bucket properties of <see cref="OperationRateLimitingRuleDefinition"/>.
/// </summary>
public static class OperationRateLimitingRuleDefinitionTokenBucketExtensions
{
    private const string TokensPerPeriodProperty = "TokenBucket:TokensPerPeriod";

    public static int GetTokensPerPeriod(this OperationRateLimitingRuleDefinition definition)
    {
        return OperationRateLimitingRuleDefinitionValueHelper.GetInt(definition, TokensPerPeriodProperty, 1);
    }

    public static void SetTokensPerPeriod(this OperationRateLimitingRuleDefinition definition, int tokensPerPeriod)
    {
        definition.WithProperty(
            TokensPerPeriodProperty,
            OperationRateLimitingRuleDefinitionValueHelper.FormatInt(tokensPerPeriod));
    }

    public static void Validate(this OperationRateLimitingRuleDefinition definition, string policyName)
    {
        if (definition.GetTokensPerPeriod() <= 0)
        {
            throw new AbpException($"The number of tokens per period of a token bucket rule in operation rate limiting policy '{policyName}' must be greater than zero.");
        }
    }
}

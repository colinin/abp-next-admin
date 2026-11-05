using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.OperationRateLimiting;

/// <summary>
/// In-memory dynamic policy source used to test the dynamic policy merge.
/// </summary>
public class FakeOperationRateLimitingPolicyStoreState
{
    public Dictionary<string, OperationRateLimitingPolicy> Policies { get; } =
        new(StringComparer.OrdinalIgnoreCase);

    public FakeOperationRateLimitingPolicyStoreState()
    {
        Policies[TestPolicyNames.DynamicOnly] = new OperationRateLimitingPolicy(TestPolicyNames.DynamicOnly)
        {
            Rules =
            {
                CreateFixedWindowRule(maxCount: 1)
            }
        };

        // Overrides the statically defined "Test.Merged" policy (max count 1 -> 3).
        Policies[TestPolicyNames.Merged] = new OperationRateLimitingPolicy(TestPolicyNames.Merged)
        {
            ErrorCode = "Test:Merged",
            Rules =
            {
                CreateFixedWindowRule(maxCount: 3)
            }
        };
    }

    private static OperationRateLimitingRuleDefinition CreateFixedWindowRule(int maxCount)
    {
        var rule = new OperationRateLimitingRuleDefinition
        {
            Algorithm = OperationRateLimitingAlgorithms.FixedWindow,
            PartitionType = OperationRateLimitingPartitionType.Parameter,
            MaxCount = maxCount
        };

        return rule;
    }
}

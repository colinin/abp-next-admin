using System;
using LINGYUN.Abp.OperationRateLimiting.Algorithms.Concurrency;
using LINGYUN.Abp.OperationRateLimiting.Algorithms.FixedWindow;
using LINGYUN.Abp.OperationRateLimiting.Algorithms.SlidingWindow;
using LINGYUN.Abp.OperationRateLimiting.Algorithms.TokenBucket;
using Shouldly;
using Xunit;

namespace LINGYUN.Abp.OperationRateLimiting;

public class OperationRateLimitingRuleDefinitionExtensionsTests
{
    [Fact]
    public void Fixed_Window_Should_Use_The_Common_Properties()
    {
        var rule = new OperationRateLimitingRuleDefinition
        {
            Algorithm = OperationRateLimitingAlgorithms.FixedWindow,
            Duration = TimeSpan.FromSeconds(5),
            MaxCount = 2
        };

        rule.Duration.ShouldBe(TimeSpan.FromSeconds(5));
        rule.MaxCount.ShouldBe(2);
        rule.Properties.ShouldBeEmpty();
        OperationRateLimitingRuleDefinitionFixedWindowExtensions.GetKeyDescriptor(rule).ShouldNotBeNullOrEmpty();
    }

    [Fact]
    public void Sliding_Window_Segments_Should_Use_Prefixed_Properties()
    {
        var rule = new OperationRateLimitingRuleDefinition
        {
            Algorithm = OperationRateLimitingAlgorithms.SlidingWindow,
            Duration = TimeSpan.FromMinutes(1),
            MaxCount = 10
        };

        OperationRateLimitingRuleDefinitionSlidingWindowExtensions.SetSegments(rule, 5);

        rule.GetPropertyOrNull("SlidingWindow:Segments").ShouldBe("5");
        OperationRateLimitingRuleDefinitionSlidingWindowExtensions.GetSegments(rule).ShouldBe(5);
    }

    [Fact]
    public void Token_Bucket_Tokens_Should_Use_Prefixed_Properties()
    {
        var rule = new OperationRateLimitingRuleDefinition
        {
            Algorithm = OperationRateLimitingAlgorithms.TokenBucket,
            Duration = TimeSpan.FromSeconds(10),
            MaxCount = 3
        };

        OperationRateLimitingRuleDefinitionTokenBucketExtensions.SetTokensPerPeriod(rule, 2);

        rule.GetPropertyOrNull("TokenBucket:TokensPerPeriod").ShouldBe("2");
        OperationRateLimitingRuleDefinitionTokenBucketExtensions.GetTokensPerPeriod(rule).ShouldBe(2);
    }

    [Fact]
    public void Concurrency_Should_Use_The_Common_Properties()
    {
        var rule = new OperationRateLimitingRuleDefinition
        {
            Algorithm = OperationRateLimitingAlgorithms.Concurrency,
            Duration = TimeSpan.FromSeconds(30),
            MaxCount = 4
        };

        rule.Duration.ShouldBe(TimeSpan.FromSeconds(30));
        rule.MaxCount.ShouldBe(4);
        rule.Properties.ShouldBeEmpty();
        OperationRateLimitingRuleDefinitionConcurrencyExtensions.GetKeyDescriptor(rule).ShouldNotBeNullOrEmpty();
    }
}

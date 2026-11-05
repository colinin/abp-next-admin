using System;
using System.Threading.Tasks;
using LINGYUN.Abp.OperationRateLimiting.Algorithms.Concurrency;
using LINGYUN.Abp.OperationRateLimiting.Algorithms.FixedWindow;
using LINGYUN.Abp.OperationRateLimiting.Algorithms.SlidingWindow;
using LINGYUN.Abp.OperationRateLimiting.Algorithms.TokenBucket;
using Microsoft.Extensions.Options;
using Shouldly;
using Volo.Abp;
using Xunit;

namespace LINGYUN.Abp.OperationRateLimiting;

public class OperationRateLimitingOptionsTests
{
    [Fact]
    public void AddPolicy_Should_Throw_When_No_Rule_Is_Defined()
    {
        var options = CreateOptions();

        Should.Throw<AbpException>(() => options.AddPolicy("NoRule", _ => { }));
    }

    [Fact]
    public void AddPolicy_Should_Throw_On_Duplicate_Rule_Names()
    {
        var options = CreateOptions();

        Should.Throw<AbpException>(() => options.AddPolicy("DuplicateNames", policy =>
        {
            policy.AddRule(rule => rule
                .WithName("Rule")
                .WithFixedWindow(TimeSpan.FromMinutes(1), 1)
                .PartitionByParameter());
            policy.AddRule(rule => rule
                .WithName("Rule")
                .WithFixedWindow(TimeSpan.FromMinutes(2), 1)
                .PartitionByParameter());
        }));
    }

    [Fact]
    public void AddPolicy_Should_Throw_On_Duplicate_Definitions()
    {
        var options = CreateOptions();

        Should.Throw<AbpException>(() => options.AddPolicy("DuplicateDefinitions", policy =>
        {
            policy.AddRule(rule => rule
                .WithFixedWindow(TimeSpan.FromMinutes(1), 1)
                .PartitionByParameter());
            policy.AddRule(rule => rule
                .WithFixedWindow(TimeSpan.FromMinutes(1), 1)
                .PartitionByParameter());
        }));
    }

    [Fact]
    public void AddPolicy_Should_Throw_When_The_Custom_Partition_Resolver_Is_Unknown()
    {
        var options = CreateOptions();

        Should.Throw<AbpException>(() => options.AddPolicy("UnknownResolver", policy =>
        {
            policy.WithFixedWindow(TimeSpan.FromMinutes(1), 1).PartitionBy("Unknown");
        }));
    }

    [Fact]
    public void AddPolicy_Should_Throw_On_Invalid_Rule_Values()
    {
        var options = CreateOptions();

        Should.Throw<AbpException>(() => options.AddPolicy("Negative", policy =>
            policy.WithFixedWindow(TimeSpan.FromMinutes(1), -1).PartitionByParameter()));

        Should.Throw<AbpException>(() => options.AddPolicy("ZeroDuration", policy =>
            policy.WithFixedWindow(TimeSpan.Zero, 1).PartitionByParameter()));
    }

    [Fact]
    public void ConfigurePolicy_Should_Throw_When_The_Policy_Does_Not_Exist()
    {
        var options = CreateOptions();

        Should.Throw<AbpException>(() => options.ConfigurePolicy("Missing", _ => { }));
    }

    [Fact]
    public void ConfigurePolicy_Should_Keep_The_Existing_Rules()
    {
        var options = CreateOptions();

        options.AddPolicy("Policy", policy =>
            policy.WithFixedWindow(TimeSpan.FromMinutes(1), 1).PartitionByParameter());

        options.ConfigurePolicy("Policy", policy => policy.WithErrorCode("Test:Policy"));

        var effective = options.GetPolicy("Policy");
        effective.ErrorCode.ShouldBe("Test:Policy");
        effective.Rules.Count.ShouldBe(1);
    }

    [Fact]
    public void ConfigurePolicy_Should_Support_ClearRules()
    {
        var options = CreateOptions();

        options.AddPolicy("Policy", policy =>
            policy.WithFixedWindow(TimeSpan.FromMinutes(1), 1).PartitionByParameter());

        options.ConfigurePolicy("Policy", policy =>
        {
            policy.ClearRules();
            policy.WithFixedWindow(TimeSpan.FromHours(1), 10).PartitionByParameter();
        });

        var effective = options.GetPolicy("Policy");
        effective.Rules.Count.ShouldBe(1);
        effective.Rules[0].MaxCount.ShouldBe(10);
    }

    [Fact]
    public void AddPolicy_Should_Replace_A_Policy_With_The_Same_Name()
    {
        var options = CreateOptions();

        options.AddPolicy("Policy", policy =>
            policy.WithFixedWindow(TimeSpan.FromMinutes(1), 1).PartitionByParameter());

        options.AddPolicy("Policy", policy =>
            policy.WithFixedWindow(TimeSpan.FromMinutes(1), 5).PartitionByParameter());

        options.GetPolicy("Policy").Rules[0].MaxCount.ShouldBe(5);
    }

    [Fact]
    public void RemovePolicy_Should_Remove_The_Policy()
    {
        var options = CreateOptions();

        options.AddPolicy("Policy", policy =>
            policy.WithFixedWindow(TimeSpan.FromMinutes(1), 1).PartitionByParameter());

        options.RemovePolicy("Policy");

        options.GetPolicyOrNull("Policy").ShouldBeNull();
    }

    [Fact]
    public void AddPartitionKeyResolver_Should_Throw_On_Duplicate_Names()
    {
        var options = CreateOptions();

        options.AddPartitionKeyResolver("Resolver", _ => Task.FromResult<string?>("a"));

        Should.Throw<AbpException>(() =>
            options.AddPartitionKeyResolver("Resolver", _ => Task.FromResult<string?>("b")));

        options.ReplacePartitionKeyResolver("Resolver", _ => Task.FromResult<string?>("b"));

        options.GetPartitionKeyResolverOrNull("Resolver").ShouldNotBeNull();
    }

    [Fact]
    public void Rule_Builder_Should_Store_Algorithm_Values_In_Prefixed_Properties()
    {
        var options = CreateOptions();

        options.AddPolicy("Prefixed", policy =>
            policy.WithSlidingWindow(TimeSpan.FromSeconds(30), maxCount: 5, segments: 6)
                .PartitionByParameter());

        var rule = options.GetPolicy("Prefixed").Rules[0];

        rule.Algorithm.ShouldBe(OperationRateLimitingAlgorithms.SlidingWindow);
        rule.Duration.ShouldBe(TimeSpan.FromSeconds(30));
        rule.MaxCount.ShouldBe(5);
        rule.GetPropertyOrNull("SlidingWindow:Segments").ShouldBe("6");
        OperationRateLimitingRuleDefinitionSlidingWindowExtensions.GetSegments(rule).ShouldBe(6);

        rule.WithProperty("MyAlgorithm:Key", "value");
        rule.GetPropertyOrNull("MyAlgorithm:Key").ShouldBe("value");
    }

    [Fact]
    public async Task Default_Static_Policy_Store_Should_Read_The_Options()
    {
        var options = CreateOptions();

        options.AddPolicy("StaticPolicy", policy =>
            policy.WithFixedWindow(TimeSpan.FromMinutes(1), 3).PartitionByParameter());

        var store = new OptionsOperationRateLimitingPolicyStore(Options.Create(options));

        var policy = await store.GetOrNullAsync("StaticPolicy");
        policy.ShouldNotBeNull();
        policy!.Rules[0].MaxCount.ShouldBe(3);

        (await store.GetPoliciesAsync()).Count.ShouldBe(1);
        (await store.GetOrNullAsync("Missing")).ShouldBeNull();
    }

    private static AbpOperationRateLimitingOptions CreateOptions()
    {
        var options = new AbpOperationRateLimitingOptions();

        options.AddAlgorithm<FixedWindowRateLimitingRule>(OperationRateLimitingAlgorithms.FixedWindow);
        options.AddAlgorithm<SlidingWindowRateLimitingRule>(OperationRateLimitingAlgorithms.SlidingWindow);
        options.AddAlgorithm<TokenBucketRateLimitingRule>(OperationRateLimitingAlgorithms.TokenBucket);
        options.AddAlgorithm<ConcurrencyRateLimitingRule>(OperationRateLimitingAlgorithms.Concurrency);

        return options;
    }
}

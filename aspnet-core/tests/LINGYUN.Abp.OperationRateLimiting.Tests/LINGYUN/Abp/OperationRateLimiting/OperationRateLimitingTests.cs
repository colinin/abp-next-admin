using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Shouldly;
using Volo.Abp;
using Volo.Abp.MultiTenancy;
using Xunit;

namespace LINGYUN.Abp.OperationRateLimiting;

public class OperationRateLimitingTests : AbpOperationRateLimitingTestBase
{
    protected IOperationRateLimitingChecker Checker { get; }

    protected ICurrentTenant CurrentTenant { get; }

    public OperationRateLimitingTests()
    {
        Checker = GetRequiredService<IOperationRateLimitingChecker>();
        CurrentTenant = GetRequiredService<ICurrentTenant>();
    }

    [Fact]
    public async Task Check_Async_Should_Allow_MaxCount_Then_Throw()
    {
        var key = UniqueKey();

        await Checker.CheckAsync(TestPolicyNames.Fixed, key);
        await Checker.CheckAsync(TestPolicyNames.Fixed, key);

        var exception = await Should.ThrowAsync<AbpOperationRateLimitingException>(
            () => Checker.CheckAsync(TestPolicyNames.Fixed, key));

        exception.HttpStatusCode.ShouldBe(429);
        exception.Code.ShouldBe(AbpOperationRateLimitingErrorCodes.ExceedLimit);
        exception.PolicyName.ShouldBe(TestPolicyNames.Fixed);
        exception.Data["PolicyName"].ShouldBe(TestPolicyNames.Fixed);
        ((int)exception.Data["MaxCount"]!).ShouldBe(2);
        ((int)exception.Data["CurrentCount"]!).ShouldBe(2);
        ((int)exception.Data["RemainingCount"]!).ShouldBe(0);
        exception.Status.RetryAfter.ShouldNotBeNull();
        exception.Status.RetryAfter!.Value.ShouldBeGreaterThan(TimeSpan.Zero);
        exception.Data.Contains("RetryAfter").ShouldBeTrue();
        exception.Status.Rules.Count.ShouldBe(1);

        await Task.Delay(400);

        await Checker.CheckAsync(TestPolicyNames.Fixed, key);
    }

    [Fact]
    public async Task IsAllowed_And_GetStatus_Should_Not_Consume()
    {
        var key = UniqueKey();

        (await Checker.IsAllowedAsync(TestPolicyNames.Fixed, key)).ShouldBeTrue();

        var status = await Checker.GetStatusAsync(TestPolicyNames.Fixed, key);
        status.IsAllowed.ShouldBeTrue();
        status.MaxCount.ShouldBe(2);
        status.CurrentCount.ShouldBe(0);
        status.RemainingCount.ShouldBe(2);

        await Checker.CheckAsync(TestPolicyNames.Fixed, key);

        status = await Checker.GetStatusAsync(TestPolicyNames.Fixed, key);
        status.CurrentCount.ShouldBe(1);
        status.RemainingCount.ShouldBe(1);

        (await Checker.IsAllowedAsync(TestPolicyNames.Fixed, key)).ShouldBeTrue();
    }

    [Fact]
    public async Task Reset_Should_Reset_The_Counter()
    {
        var key = UniqueKey();

        await Checker.CheckAsync(TestPolicyNames.Fixed, key);
        await Checker.CheckAsync(TestPolicyNames.Fixed, key);

        await Should.ThrowAsync<AbpOperationRateLimitingException>(
            () => Checker.CheckAsync(TestPolicyNames.Fixed, key));

        await Checker.ResetAsync(TestPolicyNames.Fixed, key);

        await Checker.CheckAsync(TestPolicyNames.Fixed, key);
        await Checker.CheckAsync(TestPolicyNames.Fixed, key);
    }

    [Fact]
    public async Task Sliding_Window_Should_Allow_MaxCount_And_Reset()
    {
        var key = UniqueKey();

        await Checker.CheckAsync(TestPolicyNames.Sliding, key);
        await Checker.CheckAsync(TestPolicyNames.Sliding, key);

        var exception = await Should.ThrowAsync<AbpOperationRateLimitingException>(
            () => Checker.CheckAsync(TestPolicyNames.Sliding, key));

        exception.Status.Rules[0].MaxCount.ShouldBe(2);

        await Task.Delay(550);

        await Checker.CheckAsync(TestPolicyNames.Sliding, key);
    }

    [Fact]
    public async Task Token_Bucket_Should_Allow_Burst_And_Replenish()
    {
        var key = UniqueKey();

        await Checker.CheckAsync(TestPolicyNames.TokenBucket, key);
        await Checker.CheckAsync(TestPolicyNames.TokenBucket, key);

        var exception = await Should.ThrowAsync<AbpOperationRateLimitingException>(
            () => Checker.CheckAsync(TestPolicyNames.TokenBucket, key));

        ((int)exception.Data["RetryAfterSeconds"]!).ShouldBeGreaterThanOrEqualTo(0);

        await Task.Delay(350);

        await Checker.CheckAsync(TestPolicyNames.TokenBucket, key);
    }

    [Fact]
    public async Task Concurrency_Should_Block_When_Permit_Limit_Is_Reached_And_Release()
    {
        var key = UniqueKey();

        await Checker.CheckAsync(TestPolicyNames.Concurrency, key);
        await Checker.CheckAsync(TestPolicyNames.Concurrency, key);

        await Should.ThrowAsync<AbpOperationRateLimitingException>(
            () => Checker.CheckAsync(TestPolicyNames.Concurrency, key));

        await Checker.ReleaseAsync(TestPolicyNames.Concurrency, key);

        await Checker.CheckAsync(TestPolicyNames.Concurrency, key);
    }

    [Fact]
    public async Task Concurrency_Should_Release_Permits_With_A_Lease()
    {
        var key = UniqueKey();

        var lease = await Checker.AcquireAsync(TestPolicyNames.Concurrency, key);
        await Checker.CheckAsync(TestPolicyNames.Concurrency, key);

        await Should.ThrowAsync<AbpOperationRateLimitingException>(
            () => Checker.CheckAsync(TestPolicyNames.Concurrency, key));

        await lease.ReleaseAsync();

        await Checker.CheckAsync(TestPolicyNames.Concurrency, key);
    }

    [Fact]
    public async Task Email_Partition_Should_Be_Normalized()
    {
        await Checker.CheckAsync(TestPolicyNames.Email, "User@Example.COM");

        await Should.ThrowAsync<AbpOperationRateLimitingException>(
            () => Checker.CheckAsync(TestPolicyNames.Email, "user@example.com"));
    }

    [Fact]
    public async Task PhoneNumber_Partition_Should_Be_Normalized()
    {
        await Checker.CheckAsync(TestPolicyNames.PhoneNumber, "+86 138-0013-8000");

        await Should.ThrowAsync<AbpOperationRateLimitingException>(
            () => Checker.CheckAsync(TestPolicyNames.PhoneNumber, "+8613800138000"));
    }

    [Fact]
    public async Task Multi_Rule_Should_Not_Consume_Quota_When_A_Rule_Is_Already_Blocked()
    {
        var key = UniqueKey();

        await Checker.CheckAsync(TestPolicyNames.MultiRule, key);

        await Should.ThrowAsync<AbpOperationRateLimitingException>(
            () => Checker.CheckAsync(TestPolicyNames.MultiRule, key));

        var status = await Checker.GetStatusAsync(TestPolicyNames.MultiRule, key);
        var tenantRule = status.Rules.Single(rule => rule.MaxCount == 5);

        tenantRule.CurrentCount.ShouldBe(1);
        tenantRule.RemainingCount.ShouldBe(4);
    }

    [Fact]
    public async Task Multi_Rule_Should_Require_All_Rules_To_Allow()
    {
        var key = UniqueKey();

        await Checker.CheckAsync(TestPolicyNames.Combined, key);

        await Should.ThrowAsync<AbpOperationRateLimitingException>(
            () => Checker.CheckAsync(TestPolicyNames.Combined, key));
    }

    [Fact]
    public async Task Ban_Policy_Should_Deny_Permanently()
    {
        var exception = await Should.ThrowAsync<AbpOperationRateLimitingException>(
            () => Checker.CheckAsync(TestPolicyNames.Ban, UniqueKey()));

        exception.Code.ShouldBe(AbpOperationRateLimitingErrorCodes.ExceedLimitPermanently);
        exception.Data["RetryAfterSeconds"].ShouldBe(0);
        exception.Data.Contains("RetryAfter").ShouldBeFalse();
        exception.Status.RetryAfter.ShouldBeNull();
        exception.Status.Rules[0].IsPermanent.ShouldBeTrue();
    }

    [Fact]
    public async Task Policy_Error_Code_Should_Be_Used()
    {
        var key = UniqueKey();

        await Checker.CheckAsync(TestPolicyNames.CustomError, key);

        var exception = await Should.ThrowAsync<AbpOperationRateLimitingException>(
            () => Checker.CheckAsync(TestPolicyNames.CustomError, key));

        exception.Code.ShouldBe("Test:CustomError");
    }

    [Fact]
    public async Task Extra_Properties_Should_Be_Copied_To_The_Exception_Data()
    {
        var key = UniqueKey();
        var context = new OperationRateLimitingContext
        {
            Parameter = key,
            Properties =
            {
                ["DeviceId"] = "device-1"
            }
        };

        await Checker.CheckAsync(TestPolicyNames.Fixed, context);
        await Checker.CheckAsync(TestPolicyNames.Fixed, context);

        var exception = await Should.ThrowAsync<AbpOperationRateLimitingException>(
            () => Checker.CheckAsync(TestPolicyNames.Fixed, new OperationRateLimitingContext
            {
                Parameter = key,
                Properties =
                {
                    ["DeviceId"] = "device-1"
                }
            }));

        exception.Data["DeviceId"].ShouldBe("device-1");
    }

    [Fact]
    public async Task Custom_Partition_Resolver_Should_Separate_Counters()
    {
        var context = new OperationRateLimitingContext
        {
            Parameter = "api",
            Properties = { ["DeviceId"] = "device-1" }
        };

        await Checker.CheckAsync(TestPolicyNames.CustomPartition, context);

        await Should.ThrowAsync<AbpOperationRateLimitingException>(
            () => Checker.CheckAsync(TestPolicyNames.CustomPartition, new OperationRateLimitingContext
            {
                Parameter = "api",
                Properties = { ["DeviceId"] = "device-1" }
            }));

        await Checker.CheckAsync(TestPolicyNames.CustomPartition, new OperationRateLimitingContext
        {
            Parameter = "api",
            Properties = { ["DeviceId"] = "device-2" }
        });
    }

    [Fact]
    public async Task Disabled_Policy_Should_Allow_Everything()
    {
        var status = await Checker.GetStatusAsync(TestPolicyNames.DisabledPolicy, UniqueKey());

        status.IsAllowed.ShouldBeTrue();
        status.MaxCount.ShouldBe(int.MaxValue);
        status.RemainingCount.ShouldBe(int.MaxValue);
    }

    [Fact]
    public async Task Globally_Disabled_Rate_Limiting_Should_Allow_Everything()
    {
        var options = GetRequiredService<IOptions<AbpOperationRateLimitingOptions>>().Value;
        var key = UniqueKey();

        options.IsEnabled = false;
        try
        {
            for (var index = 0; index < 3; index++)
            {
                var status = await Checker.CheckAsync(TestPolicyNames.Ban, key);
                status.MaxCount.ShouldBe(int.MaxValue);
                status.RemainingCount.ShouldBe(int.MaxValue);
                status.CurrentCount.ShouldBe(0);
            }

            (await Checker.IsAllowedAsync(TestPolicyNames.Ban, key)).ShouldBeTrue();
        }
        finally
        {
            options.IsEnabled = true;
        }
    }

    [Fact]
    public async Task Tenant_Isolation_Should_Create_Independent_Counters()
    {
        var key = UniqueKey();
        var tenant1 = Guid.NewGuid();
        var tenant2 = Guid.NewGuid();

        using (CurrentTenant.Change(tenant1))
        {
            await Checker.CheckAsync(TestPolicyNames.Tenant, key);
        }

        using (CurrentTenant.Change(tenant2))
        {
            await Checker.CheckAsync(TestPolicyNames.Tenant, key);
        }

        using (CurrentTenant.Change(tenant1))
        {
            await Should.ThrowAsync<AbpOperationRateLimitingException>(
                () => Checker.CheckAsync(TestPolicyNames.Tenant, key));
        }
    }

    [Fact]
    public async Task Dynamic_Policies_Should_Be_Merged_With_Static_Policies()
    {
        var dynamicOnlyKey = UniqueKey();

        await Checker.CheckAsync(TestPolicyNames.DynamicOnly, dynamicOnlyKey);
        await Should.ThrowAsync<AbpOperationRateLimitingException>(
            () => Checker.CheckAsync(TestPolicyNames.DynamicOnly, dynamicOnlyKey));

        // The static definition allows 1 request, the dynamic definition overrides it with 3.
        var mergedKey = UniqueKey();

        await Checker.CheckAsync(TestPolicyNames.Merged, mergedKey);
        await Checker.CheckAsync(TestPolicyNames.Merged, mergedKey);
        var third = await Checker.CheckAsync(TestPolicyNames.Merged, mergedKey);
        third.RemainingCount.ShouldBe(0);

        var exception = await Should.ThrowAsync<AbpOperationRateLimitingException>(
            () => Checker.CheckAsync(TestPolicyNames.Merged, mergedKey));

        exception.Code.ShouldBe("Test:Merged");
    }

    [Fact]
    public async Task Attribute_Should_Enforce_The_Policy_On_Methods()
    {
        var service = GetRequiredService<TestOperationRateLimitingService>();
        var key = UniqueKey();

        await service.InvokeAsync(key);

        await Should.ThrowAsync<AbpOperationRateLimitingException>(
            () => service.InvokeAsync(key));

        service.InvokedCount.ShouldBe(1);

        await service.InvokeAsync(UniqueKey());
        service.InvokedCount.ShouldBe(2);
    }

    [Fact]
    public async Task Attribute_Should_Resolve_The_Parameter_From_A_Dto()
    {
        var service = GetRequiredService<TestOperationRateLimitingService>();

        await service.InvokeWithDtoAsync(new TestRateLimitingInput { PhoneNumber = "+8613800138000" });

        await Should.ThrowAsync<AbpOperationRateLimitingException>(
            () => service.InvokeWithDtoAsync(new TestRateLimitingInput { PhoneNumber = "+8613800138000" }));
    }

    [Fact]
    public async Task Class_Level_Attribute_Should_Be_Applied()
    {
        var service = GetRequiredService<TestClassLevelRateLimitingService>();
        var key = UniqueKey();

        await service.InvokeAsync(key);

        await Should.ThrowAsync<AbpOperationRateLimitingException>(
            () => service.InvokeAsync(key));
    }

    [Fact]
    public async Task Multiple_RateLimiting_Parameters_Should_Throw()
    {
        var service = GetRequiredService<TestOperationRateLimitingService>();

        await Should.ThrowAsync<AbpException>(
            () => service.InvokeWithTwoParametersAsync("a", "b"));
    }

    [Fact]
    public async Task Custom_Algorithm_Should_Be_Executed()
    {
        var key = UniqueKey();

        await Checker.CheckAsync(TestPolicyNames.CustomAlgorithm, key);
        await Checker.CheckAsync(TestPolicyNames.CustomAlgorithm, key);

        await Should.ThrowAsync<AbpOperationRateLimitingException>(
            () => Checker.CheckAsync(TestPolicyNames.CustomAlgorithm, key));
    }

    [Fact]
    public void Formatter_Should_Format_Durations_With_Localization()
    {
        var culture = System.Globalization.CultureInfo.CurrentUICulture;

        try
        {
            System.Globalization.CultureInfo.CurrentUICulture = new System.Globalization.CultureInfo("en");

            var formatter = GetRequiredService<IOperationRateLimitingFormatter>();

            formatter.FormatDuration(TimeSpan.FromMinutes(5)).ShouldBe("5 minutes 0 seconds");
            formatter.FormatDuration(TimeSpan.FromSeconds(30)).ShouldBe("30 seconds");
            formatter.FormatRetryAfter(null).ShouldBeEmpty();
        }
        finally
        {
            System.Globalization.CultureInfo.CurrentUICulture = culture;
        }
    }

    private static string UniqueKey()
    {
        return Guid.NewGuid().ToString("N");
    }
}

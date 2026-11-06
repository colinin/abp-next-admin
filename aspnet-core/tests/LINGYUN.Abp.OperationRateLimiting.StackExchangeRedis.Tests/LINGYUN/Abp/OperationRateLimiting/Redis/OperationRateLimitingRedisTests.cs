using LINGYUN.Abp.OperationRateLimiting.Redis.Algorithms.Concurrency;
using LINGYUN.Abp.OperationRateLimiting.Redis.Algorithms.FixedWindow;
using LINGYUN.Abp.OperationRateLimiting.Redis.Algorithms.SlidingWindow;
using LINGYUN.Abp.OperationRateLimiting.Redis.Algorithms.TokenBucket;
using Microsoft.Extensions.Options;
using Shouldly;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace LINGYUN.Abp.OperationRateLimiting.Redis;

public class OperationRateLimitingRedisTests : AbpOperationRateLimitingRedisTestBase
{
    protected IOperationRateLimitingChecker Checker { get; }

    public OperationRateLimitingRedisTests()
    {
        Checker = GetRequiredService<IOperationRateLimitingChecker>();
    }

    [Fact]
    public void BuiltIn_Rules_Should_Be_Replaced_By_Redis_Implementations()
    {
        var options = GetRequiredService<IOptions<AbpOperationRateLimitingOptions>>().Value;

        options.GetAlgorithmType(OperationRateLimitingAlgorithms.FixedWindow)
            .ShouldBe(typeof(RedisFixedWindowRateLimitingRule));
        options.GetAlgorithmType(OperationRateLimitingAlgorithms.SlidingWindow)
            .ShouldBe(typeof(RedisSlidingWindowRateLimitingRule));
        options.GetAlgorithmType(OperationRateLimitingAlgorithms.TokenBucket)
            .ShouldBe(typeof(RedisTokenBucketRateLimitingRule));
        options.GetAlgorithmType(OperationRateLimitingAlgorithms.Concurrency)
            .ShouldBe(typeof(RedisConcurrencyRateLimitingRule));
    }

    [Fact]
    public async Task Fixed_Window_Should_Use_Redis()
    {
        var key = UniqueKey();

        await Checker.CheckAsync(TestPolicyNames.Fixed, key);
        await Checker.CheckAsync(TestPolicyNames.Fixed, key);

        await Should.ThrowAsync<AbpOperationRateLimitingException>(
            () => Checker.CheckAsync(TestPolicyNames.Fixed, key));

        await Task.Delay(400);

        await Checker.CheckAsync(TestPolicyNames.Fixed, key);
    }

    [Fact]
    public async Task Reset_Should_Delete_The_Redis_Key()
    {
        var key = UniqueKey();

        GetRequiredService<IOperationRateLimitingRuleResetter>()
            .ShouldBeOfType<StackExchangeRedisOperationRateLimitingRuleReleaser>();

        await Checker.CheckAsync(TestPolicyNames.Fixed, key);
        await Checker.CheckAsync(TestPolicyNames.Fixed, key);

        await Should.ThrowAsync<AbpOperationRateLimitingException>(
            () => Checker.CheckAsync(TestPolicyNames.Fixed, key));

        await Checker.ResetAsync(TestPolicyNames.Fixed, key);

        await Checker.CheckAsync(TestPolicyNames.Fixed, key);
    }

    [Fact]
    public async Task Concurrency_Should_Release_With_Redis()
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
    public async Task Ban_Policy_Should_Not_Touch_Redis()
    {
        var exception = await Should.ThrowAsync<AbpOperationRateLimitingException>(
            () => Checker.CheckAsync(TestPolicyNames.Ban, UniqueKey()));

        exception.Code.ShouldBe(AbpOperationRateLimitingErrorCodes.ExceedLimitPermanently);
    }

    [Fact]
    public async Task Lua_Script_Should_Enforce_The_Limit_Atomically()
    {
        var key = UniqueKey();
        var successes = 0;

        var tasks = Enumerable.Range(0, 50).Select(async _ =>
        {
            try
            {
                await Checker.CheckAsync(RedisTestPolicyNames.Atomic, key);
                Interlocked.Increment(ref successes);
            }
            catch (AbpOperationRateLimitingException)
            {
                // expected
            }
        });

        await Task.WhenAll(tasks);

        successes.ShouldBe(10);
    }

    private static string UniqueKey()
    {
        return Guid.NewGuid().ToString("N");
    }
}

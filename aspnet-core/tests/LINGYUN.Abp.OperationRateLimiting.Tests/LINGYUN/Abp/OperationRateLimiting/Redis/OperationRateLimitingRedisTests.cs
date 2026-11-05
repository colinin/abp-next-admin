using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp;
using Xunit;

namespace LINGYUN.Abp.OperationRateLimiting.Redis;

public class OperationRateLimitingRedisTests : LINGYUN.Abp.Tests.AbpTestsBase<AbpOperationRateLimitingRedisTestModule>
{
    protected IOperationRateLimitingChecker Checker { get; }

    protected IOperationRateLimitingStore Store { get; }

    public OperationRateLimitingRedisTests()
    {
        Checker = GetRequiredService<IOperationRateLimitingChecker>();
        Store = GetRequiredService<IOperationRateLimitingStore>();
    }

    [Fact]
    public void Redis_Store_Should_Be_Used()
    {
        Store.ShouldBeOfType<StackExchangeRedisOperationRateLimitingStore>();
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

    [Fact]
    public async Task Custom_State_Should_Be_Updated_Atomically()
    {
        var key = "cas:" + UniqueKey();

        var tasks = Enumerable.Range(0, 50).Select(_ => Store.UpdateStateAsync(
            key,
            (RedisTestCounterState? current) =>
            {
                current ??= new RedisTestCounterState();
                current.Count++;
                return current;
            },
            TimeSpan.FromMinutes(1)));

        await Task.WhenAll(tasks);

        var state = await Store.GetStateAsync<RedisTestCounterState>(key);

        state.ShouldNotBeNull();
        state!.Count.ShouldBe(50);

        await Store.ResetAsync(key);
        (await Store.GetStateAsync<RedisTestCounterState>(key)).ShouldBeNull();
    }

    private static string UniqueKey()
    {
        return Guid.NewGuid().ToString("N");
    }
}

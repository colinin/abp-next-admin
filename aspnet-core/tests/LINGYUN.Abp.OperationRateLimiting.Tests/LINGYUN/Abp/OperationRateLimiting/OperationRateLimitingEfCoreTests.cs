using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;
using Xunit;

namespace LINGYUN.Abp.OperationRateLimiting;

public class OperationRateLimitingEfCoreTests : AbpOperationRateLimitingEfCoreTestBase
{
    protected IOperationRateLimitingPolicyRecordRepository Repository { get; }

    protected IDynamicOperationRateLimitingPolicyStore DynamicStore { get; }

    protected IOperationRateLimitingChecker Checker { get; }

    public OperationRateLimitingEfCoreTests()
    {
        Repository = GetRequiredService<IOperationRateLimitingPolicyRecordRepository>();
        DynamicStore = GetRequiredService<IDynamicOperationRateLimitingPolicyStore>();
        Checker = GetRequiredService<IOperationRateLimitingChecker>();
    }

    [Fact]
    public async Task Should_Load_Dynamic_Policies_From_The_Repository()
    {
        var name = "Ef." + Guid.NewGuid().ToString("N");

        await WithUnitOfWorkAsync(async () =>
        {
            var record = new OperationRateLimitingPolicyRecord(Guid.NewGuid(), name);
            var rule = new OperationRateLimitingRuleRecord(Guid.NewGuid(), record.Id, OperationRateLimitingAlgorithms.FixedWindow)
            {
                PartitionType = OperationRateLimitingPartitionType.Parameter,
                MaxCount = 1,
                SortOrder = 0
            };
            record.SetRules(new[] { rule });

            await Repository.InsertAsync(record);
        });

        await DynamicStore.InvalidateCacheAsync();

        DynamicStore.ShouldBeOfType<DynamicOperationRateLimitingPolicyStore>();

        await WithUnitOfWorkAsync(async () =>
        {
            var record = await Repository.FindByNameAsync(name);
            record.ShouldNotBeNull();
            record!.Rules.Count.ShouldBe(1);
        });

        var policies = await DynamicStore.GetPoliciesAsync();
        policies.Select(policy => policy.Name).ShouldContain(name);

        var policy = await DynamicStore.GetOrNullAsync(name);

        policy.ShouldNotBeNull();
        policy!.Rules.Count.ShouldBe(1);
        policy.Rules[0].MaxCount.ShouldBe(1);

        var key = Guid.NewGuid().ToString("N");
        await Checker.CheckAsync(name, key);
        await Should.ThrowAsync<AbpOperationRateLimitingException>(() => Checker.CheckAsync(name, key));
    }

    [Fact]
    public async Task Repository_Should_Find_A_Policy_By_Name()
    {
        var name = "Ef.Find." + Guid.NewGuid().ToString("N");

        await WithUnitOfWorkAsync(async () =>
        {
            await Repository.InsertAsync(new OperationRateLimitingPolicyRecord(Guid.NewGuid(), name));
        });

        await WithUnitOfWorkAsync(async () =>
        {
            var record = await Repository.FindByNameAsync(name);

            record.ShouldNotBeNull();
            record!.Name.ShouldBe(name);
        });
    }
}

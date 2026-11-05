using System;
using System.Linq;
using System.Threading.Tasks;
using LINGYUN.Abp.OperationRateLimiting.Management;
using LINGYUN.Abp.OperationRateLimiting.Management.Dto;
using Shouldly;
using Volo.Abp;
using Xunit;

namespace LINGYUN.Abp.OperationRateLimiting;

public class OperationRateLimitingManagementTests : LINGYUN.Abp.Tests.AbpTestsBase<AbpOperationRateLimitingManagementTestModule>
{
    protected IOperationRateLimitingPolicyAppService PolicyAppService { get; }

    protected IOperationRateLimitingChecker Checker { get; }

    public OperationRateLimitingManagementTests()
    {
        PolicyAppService = GetRequiredService<IOperationRateLimitingPolicyAppService>();
        Checker = GetRequiredService<IOperationRateLimitingChecker>();
    }

    [Fact]
    public async Task Should_Create_Update_And_Delete_A_Policy()
    {
        var name = "Mgmt." + Guid.NewGuid().ToString("N");

        var created = await PolicyAppService.CreateAsync(new CreateOperationRateLimitingPolicyDto
        {
            Name = name,
            DisplayName = "Management test",
            Rules =
            {
                new OperationRateLimitingRuleDto
                {
                    Algorithm = OperationRateLimitingAlgorithms.FixedWindow,
                    DurationSeconds = 60,
                    MaxCount = 1,
                    PartitionType = OperationRateLimitingPartitionType.Parameter
                }
            }
        });

        created.Id.ShouldNotBe(Guid.Empty);
        created.Rules.Count.ShouldBe(1);

        var loaded = await PolicyAppService.GetByNameAsync(name);
        loaded.Id.ShouldBe(created.Id);

        var updated = await PolicyAppService.UpdateAsync(created.Id, new UpdateOperationRateLimitingPolicyDto
        {
            DisplayName = "Updated",
            IsEnabled = false,
            Rules =
            {
                new OperationRateLimitingRuleDto
                {
                    Algorithm = OperationRateLimitingAlgorithms.SlidingWindow,
                    DurationSeconds = 120,
                    MaxCount = 5,
                    PartitionType = OperationRateLimitingPartitionType.Parameter
                }
            }
        });

        updated.DisplayName.ShouldBe("Updated");
        updated.IsEnabled.ShouldBeFalse();
        updated.Rules[0].Algorithm.ShouldBe(OperationRateLimitingAlgorithms.SlidingWindow);

        var list = await PolicyAppService.GetListAsync(new OperationRateLimitingPolicyGetListInput
        {
            Filter = name,
            MaxResultCount = 10
        });
        list.TotalCount.ShouldBe(1);

        await PolicyAppService.DeleteAsync(created.Id);

        var exception = await Should.ThrowAsync<Volo.Abp.Domain.Entities.EntityNotFoundException>(
            () => PolicyAppService.GetAsync(created.Id));
        exception.ShouldNotBeNull();
    }

    [Fact]
    public async Task Created_Policy_Should_Be_Effective_Immediately()
    {
        var name = "Mgmt.Effective." + Guid.NewGuid().ToString("N");

        await PolicyAppService.CreateAsync(new CreateOperationRateLimitingPolicyDto
        {
            Name = name,
            Rules =
            {
                new OperationRateLimitingRuleDto
                {
                    Algorithm = OperationRateLimitingAlgorithms.FixedWindow,
                    DurationSeconds = 60,
                    MaxCount = 1,
                    PartitionType = OperationRateLimitingPartitionType.Parameter
                }
            }
        });

        var key = Guid.NewGuid().ToString("N");

        await Checker.CheckAsync(name, key);

        await Should.ThrowAsync<AbpOperationRateLimitingException>(
            () => Checker.CheckAsync(name, key));
    }

    [Fact]
    public async Task Create_Should_Reject_Duplicate_Names()
    {
        var name = "Mgmt.Duplicate." + Guid.NewGuid().ToString("N");

        await PolicyAppService.CreateAsync(new CreateOperationRateLimitingPolicyDto
        {
            Name = name,
            Rules =
            {
                new OperationRateLimitingRuleDto
                {
                    Algorithm = OperationRateLimitingAlgorithms.FixedWindow,
                    DurationSeconds = 60,
                    MaxCount = 1
                }
            }
        });

        var exception = await Should.ThrowAsync<BusinessException>(
            () => PolicyAppService.CreateAsync(new CreateOperationRateLimitingPolicyDto
            {
                Name = name,
                Rules =
                {
                    new OperationRateLimitingRuleDto
                    {
                        Algorithm = OperationRateLimitingAlgorithms.FixedWindow,
                        DurationSeconds = 60,
                        MaxCount = 1
                    }
                }
            }));

        exception.Code.ShouldBe(AbpOperationRateLimitingManagementErrorCodes.PolicyNameAlreadyExists);
    }

    [Fact]
    public async Task Create_Should_Reject_Unknown_Algorithms()
    {
        var name = "Mgmt.Unknown." + Guid.NewGuid().ToString("N");

        var exception = await Should.ThrowAsync<BusinessException>(
            () => PolicyAppService.CreateAsync(new CreateOperationRateLimitingPolicyDto
            {
                Name = name,
                Rules =
                {
                    new OperationRateLimitingRuleDto
                    {
                        Algorithm = "UnknownAlgorithm",
                        DurationSeconds = 60,
                        MaxCount = 1
                    }
                }
            }));

        exception.Code.ShouldBe(AbpOperationRateLimitingManagementErrorCodes.InvalidPolicy);
    }
}

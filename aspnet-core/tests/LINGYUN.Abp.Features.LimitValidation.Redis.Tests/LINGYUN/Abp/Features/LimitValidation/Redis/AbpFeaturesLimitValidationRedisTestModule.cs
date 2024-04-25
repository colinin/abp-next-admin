using LINGYUN.Abp.Tests;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Features.LimitValidation.Redis
{
    [DependsOn(
        typeof(AbpFeaturesLimitValidationTestModule),
        typeof(AbpFeaturesValidationRedisModule),
        typeof(AbpTestsBaseModule))]
    public class AbpFeaturesLimitValidationRedisTestModule : AbpModule
    {
    }
}

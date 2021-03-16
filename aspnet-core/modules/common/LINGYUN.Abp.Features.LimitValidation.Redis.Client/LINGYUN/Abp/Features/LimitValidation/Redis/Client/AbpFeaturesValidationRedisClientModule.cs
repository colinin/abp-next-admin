using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Features.LimitValidation.Redis.Client
{
    [DependsOn(typeof(AbpFeaturesValidationRedisModule))]
    public class AbpFeaturesValidationRedisClientModule : AbpModule
    {
    }
}

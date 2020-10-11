using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.Features.LimitValidation.Redis
{
    [DependsOn(
        typeof(AbpFeaturesLimitValidationModule))]
    public class AbpFeaturesValidationRedisModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            Configure<AbpRedisRequiresLimitFeatureOptions>(configuration.GetSection("Features:Validation:Redis"));

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpFeaturesValidationRedisModule>();
            });

            context.Services.Replace(ServiceDescriptor.Singleton<IRequiresLimitFeatureChecker, RedisRequiresLimitFeatureChecker>());
        }
    }
}

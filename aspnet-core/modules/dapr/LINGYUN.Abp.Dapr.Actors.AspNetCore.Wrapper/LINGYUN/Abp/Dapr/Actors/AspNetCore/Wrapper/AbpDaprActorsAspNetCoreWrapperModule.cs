using Dapr.Actors;
using LINGYUN.Abp.Wrapper;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Dapr.Actors.AspNetCore.Wrapper
{
    [DependsOn(
        typeof(AbpDaprActorsAspNetCoreModule),
        typeof(AbpWrapperModule))]
    public class AbpDaprActorsAspNetCoreWrapperModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpWrapperOptions>(options =>
            {
                options.IgnoredInterfaces.TryAdd<IActor>();
            });
        }
    }
}
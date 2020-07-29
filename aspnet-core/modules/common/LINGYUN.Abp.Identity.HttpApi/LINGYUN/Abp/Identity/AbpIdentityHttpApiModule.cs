using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Identity
{
    [DependsOn(
        typeof(Volo.Abp.Identity.AbpIdentityHttpApiModule),
        typeof(AbpIdentityApplicationContractsModule))]
    public class AbpIdentityHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpIdentityHttpApiModule).Assembly);
            });
        }
    }
}

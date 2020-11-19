using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.Identity.Localization;
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
            PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(typeof(IdentityResource), typeof(AbpIdentityApplicationContractsModule).Assembly);
                options.AddAssemblyResource(typeof(IdentityResource), typeof(Volo.Abp.Identity.AbpIdentityApplicationContractsModule).Assembly);
            });

            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpIdentityHttpApiModule).Assembly);
            });
        }
    }
}

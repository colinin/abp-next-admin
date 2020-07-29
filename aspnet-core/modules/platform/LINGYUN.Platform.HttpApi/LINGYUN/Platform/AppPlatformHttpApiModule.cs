using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace LINGYUN.Platform.HttpApi
{
    [DependsOn(
        typeof(AppPlatformApplicationContractModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class AppPlatformHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(AppPlatformApplicationContractModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<FormOptions>(options =>
            {
                options.ValueLengthLimit = int.MaxValue;
                options.MultipartBodyLengthLimit = int.MaxValue;
                options.MultipartHeadersLengthLimit = int.MaxValue;
            });
        }
    }
}

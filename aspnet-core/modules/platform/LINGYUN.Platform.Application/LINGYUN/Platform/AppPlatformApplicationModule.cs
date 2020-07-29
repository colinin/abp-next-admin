using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace LINGYUN.Platform
{
    [DependsOn(typeof(AppPlatformApplicationContractModule))]
    public class AppPlatformApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<AppPlatformApplicationModule>();

            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<AppPlatformApplicationMappingProfile>(validate: true);
            });
        }
    }
}

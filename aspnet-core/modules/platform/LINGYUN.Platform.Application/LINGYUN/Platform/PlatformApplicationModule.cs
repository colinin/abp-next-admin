using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace LINGYUN.Platform
{
    [DependsOn(typeof(PlatformApplicationContractModule))]
    public class PlatformApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<PlatformApplicationModule>();

            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<PlatformApplicationMappingProfile>(validate: true);
            });
        }
    }
}

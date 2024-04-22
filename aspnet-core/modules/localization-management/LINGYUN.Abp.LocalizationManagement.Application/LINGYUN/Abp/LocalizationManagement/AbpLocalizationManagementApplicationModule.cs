using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;

namespace LINGYUN.Abp.LocalizationManagement
{
    [DependsOn(
        typeof(AbpDddApplicationModule),
        typeof(AbpLocalizationManagementDomainModule),
        typeof(AbpLocalizationManagementApplicationContractsModule))]
    public class AbpLocalizationManagementApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<AbpLocalizationManagementApplicationModule>();

            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<LocalizationManagementApplicationMapperProfile>(validate: true);
            });
        }
    }
}

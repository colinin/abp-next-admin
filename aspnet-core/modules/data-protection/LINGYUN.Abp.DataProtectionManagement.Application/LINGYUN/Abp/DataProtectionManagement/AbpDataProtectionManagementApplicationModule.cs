using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.DataProtectionManagement;

[DependsOn(
    typeof(AbpDddApplicationModule),
    typeof(AbpDataProtectionManagementApplicationContractsModule),
    typeof(AbpDataProtectionManagementDomainModule))]
public class AbpDataProtectionManagementApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<AbpDataProtectionManagementApplicationModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<DataProtectionManagementApplicationMappingProfile>(validate: true);
        });
    }
}

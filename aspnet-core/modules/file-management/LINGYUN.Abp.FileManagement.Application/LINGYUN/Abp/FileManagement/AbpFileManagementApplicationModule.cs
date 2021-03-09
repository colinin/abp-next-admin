using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace LINGYUN.Abp.FileManagement
{
    [DependsOn(
        typeof(AbpAutoMapperModule),
        typeof(AbpFileManagementDomainModule),
        typeof(AbpFileManagementApplicationContractsModule))]
    public class AbpFileManagementApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<AbpFileManagementApplicationModule>();

            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<FileManagementApplicationAutoMapperProfile>(validate: true);
            });
        }
    }
}

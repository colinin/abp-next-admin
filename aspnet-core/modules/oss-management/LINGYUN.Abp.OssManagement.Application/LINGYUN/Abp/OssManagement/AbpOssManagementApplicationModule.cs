using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace LINGYUN.Abp.OssManagement
{
    [DependsOn(
        typeof(AbpAutoMapperModule),
        typeof(AbpOssManagementDomainModule),
        typeof(AbpOssManagementApplicationContractsModule))]
    public class AbpOssManagementApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<AbpOssManagementApplicationModule>();

            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<OssManagementApplicationAutoMapperProfile>(validate: true);
            });
        }
    }
}

using LINGYUN.ApiGateway.Localization;
using Volo.Abp.Application;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.ApiGateway
{
    [DependsOn(
        typeof(ApiGatewayDomainSharedModule), 
        typeof(AbpDddApplicationContractsModule))]
    public class ApiGatewayApplicationContractsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<ApiGatewayApplicationContractsModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<ApiGatewayResource>()
                    .AddVirtualJson("/LINGYUN/ApiGateway/Localization/ApplicationContracts");
            });
        }
    }
}

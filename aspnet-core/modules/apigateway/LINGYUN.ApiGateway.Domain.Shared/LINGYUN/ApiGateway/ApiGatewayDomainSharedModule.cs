using LINGYUN.ApiGateway.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.ApiGateway
{
    [DependsOn(typeof(AbpLocalizationModule))]
    public class ApiGatewayDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<ApiGatewayDomainSharedModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                .Add<ApiGatewayResource>("zh-Hans")
                .AddVirtualJson("/LINGYUN/ApiGateway/Localization/DomainShared");
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {

            });
        }
    }
}

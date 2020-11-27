using LINGYUN.ApiGateWay.Admin.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.ApiGateWay.Admin
{
    [DependsOn(typeof(AbpLocalizationModule))]
    public class ApiGateWayAdminDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<ApiGateWayAdminDomainSharedModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                .Add<ApiGateWayAdminResource>("zh-Hans")
                .AddVirtualJson("/LINGYUN/ApiGateWay/Admin/Localization/Resources");
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("ApiGateWayAdmin", typeof(ApiGateWayAdminResource));
            });
        }
    }
}

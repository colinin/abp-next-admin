using LINGYUN.Abp.OpenApi.ConfigurationStore;
using LINGYUN.Abp.OpenApi.Localization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.Security;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.OpenApi
{
    [DependsOn(
        typeof(AbpSecurityModule),
        typeof(AbpLocalizationModule))]
    public class AbpOpenApiModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            var openApiConfig = configuration.GetSection("OpenApi");
            Configure<AbpOpenApiOptions>(openApiConfig);
            Configure<AbpDefaultAppKeyStoreOptions>(openApiConfig);

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpOpenApiModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<OpenApiResource>()
                    .AddVirtualJson("/LINGYUN/Abp/OpenApi/Localization/Resources");
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace(AbpOpenApiConsts.KeyPrefix, typeof(OpenApiResource));
            });
        }
    }
}

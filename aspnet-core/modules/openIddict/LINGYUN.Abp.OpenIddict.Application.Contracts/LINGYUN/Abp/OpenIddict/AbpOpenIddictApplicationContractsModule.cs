using Volo.Abp.Authorization;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict;
using Volo.Abp.OpenIddict.Localization;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.OpenIddict;

[DependsOn(
    typeof(AbpAuthorizationAbstractionsModule),
    typeof(AbpOpenIddictDomainSharedModule))]
public class AbpOpenIddictApplicationContractsModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpOpenIddictApplicationContractsModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<AbpOpenIddictResource>()
                .AddVirtualJson("/LINGYUN/Abp/OpenIddict/Localization/Resources");
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace(OpenIddictApplicationErrorCodes.Namespace, typeof(AbpOpenIddictResource));
        });
    }
}

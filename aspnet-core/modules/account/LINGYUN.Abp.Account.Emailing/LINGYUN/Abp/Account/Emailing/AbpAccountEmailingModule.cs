using LINGYUN.Abp.Account.Emailing.Localization;
using Volo.Abp.Emailing;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.Account.Emailing;

[DependsOn(
    typeof(AbpEmailingModule),
    typeof(AbpUiNavigationModule))]
public class AbpAccountEmailingModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpAccountEmailingModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<AccountEmailingResource>("en")
                .AddVirtualJson("/LINGYUN/Abp/Account/Emailing/Localization/Resources");
        });
    }
}

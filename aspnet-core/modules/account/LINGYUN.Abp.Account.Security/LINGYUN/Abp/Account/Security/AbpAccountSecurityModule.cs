using LINGYUN.Abp.Account.Security.Localization;
using Volo.Abp.Emailing;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Sms;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.Account.Security;

[DependsOn(
    typeof(AbpEmailingModule),
    typeof(AbpSmsModule),
    typeof(AbpUiNavigationModule))]
public class AbpAccountSecurityModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpAccountSecurityModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<AccountSecurityResource>("en")
                .AddVirtualJson("/LINGYUN/Abp/Account/Security/Localization/Resources");
        });
    }
}
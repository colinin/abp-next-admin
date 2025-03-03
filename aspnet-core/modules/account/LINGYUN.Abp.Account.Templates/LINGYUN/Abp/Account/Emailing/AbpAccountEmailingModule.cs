using Volo.Abp.Account.Localization;
using Volo.Abp.Emailing;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.Account.Emailing;

[DependsOn(
    typeof(AbpEmailingModule), 
    typeof(AbpAccountApplicationContractsModule))]
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
                .Get<AccountResource>()
                .AddVirtualJson("/LINGYUN/Abp/Account/Emailing/Localization/Resources");
        });
    }
}

using Volo.Abp.Account.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.Account
{
    [DependsOn(typeof(AbpAccountDomainSharedModule))]
    public class AbpAccountApplicationContractsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpAccountApplicationContractsModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<Localization.AccountResource>()
                    .AddBaseTypes(typeof(AccountResource))
                    .AddVirtualJson("/LINGYUN/Abp/Account/Localization/Resources");
            });
        }
    }
}

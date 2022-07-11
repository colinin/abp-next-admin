using LINGYUN.Abp.Account.Localization;
using Volo.Abp.Emailing;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.Account
{
    [DependsOn(
        typeof(AbpEmailingModule))]
    public class AbpAccountDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpAccountDomainSharedModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<AbpAccountResource>("en")
                    .AddVirtualJson("/LINGYUN/Abp/Account/Localization/Resources");
            });
        }
    }
}

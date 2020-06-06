using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.Account
{
    [DependsOn(typeof(AbpAccountDomainSharedModule))]
    public class AbpAccountDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpAccountDomainModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<Localization.AccountResource>()
                    .AddVirtualJson("/LINGYUN/Abp/Account/Localization/Resources");
            });
        }
    }
}

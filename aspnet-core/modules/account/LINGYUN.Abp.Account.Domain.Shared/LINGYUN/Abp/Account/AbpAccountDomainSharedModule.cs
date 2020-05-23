using LINGYUN.Abp.Account.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Account
{
    [DependsOn(typeof(AbpLocalizationModule))]
    public class AbpAccountDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<AccountResource>("zh-Hans");
            });
        }
    }
}

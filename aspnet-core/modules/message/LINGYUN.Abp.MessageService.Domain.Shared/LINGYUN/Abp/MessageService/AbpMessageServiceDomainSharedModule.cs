using LINGYUN.Abp.MessageService.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.MessageService
{
    [DependsOn(typeof(AbpLocalizationModule))]
    public class AbpMessageServiceDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources.Add<MessageServiceResource>("en");
            });
        }
    }
}

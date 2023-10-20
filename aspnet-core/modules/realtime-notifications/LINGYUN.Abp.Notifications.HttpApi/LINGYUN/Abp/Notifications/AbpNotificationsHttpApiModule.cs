using LINGYUN.Abp.Notifications.Localization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Notifications;

[DependsOn(
    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpNotificationsApplicationContractsModule))]
public class AbpNotificationsHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpNotificationsHttpApiModule).Assembly);
        });

        PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(
                typeof(NotificationsResource),
                typeof(AbpNotificationsApplicationContractsModule).Assembly);  // Dto所在程序集
        });
    }
}

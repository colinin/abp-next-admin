using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Notifications;

[DependsOn(
    typeof(AbpNotificationsDomainModule),
    typeof(AbpNotificationsApplicationContractsModule),
    typeof(AbpDddApplicationModule))]
public class AbpNotificationsApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<AbpNotificationsApplicationModule>();

        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<AbpNotificationsApplicationAutoMapperProfile>(validate: true);
        });
    }
}

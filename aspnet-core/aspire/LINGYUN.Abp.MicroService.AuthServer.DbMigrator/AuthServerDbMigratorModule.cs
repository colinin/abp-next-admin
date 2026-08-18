using LINGYUN.Abp.Identity.Notifications;
using LINGYUN.Abp.Notifications.Common;
using LINGYUN.Abp.Notifications.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.MicroService.AuthServer;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpNotificationsCommonModule),
    typeof(AbpIdentityNotificationsModule),
    typeof(AbpNotificationsEntityFrameworkCoreModule),
    typeof(AuthServerMigrationsEntityFrameworkCoreModule))]
public class AuthServerDbMigratorModule : AbpModule
{
}

using LINGYUN.Abp.Identity.Notifications;
using LINGYUN.Abp.Notifications.Common;
using LINGYUN.Abp.Notifications.EntityFrameworkCore;
using LY.MicroService.AuthServer.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace LY.MicroService.AuthServer.DbMigrator;

[DependsOn(
    typeof(AuthServerMigrationsEntityFrameworkCoreModule),
    typeof(AbpNotificationsEntityFrameworkCoreModule),
    typeof(AbpNotificationsCommonModule),
    typeof(AbpIdentityNotificationsModule),
    typeof(AbpAutofacModule)
    )]
public partial class AuthServerDbMigratorModule : AbpModule
{
}

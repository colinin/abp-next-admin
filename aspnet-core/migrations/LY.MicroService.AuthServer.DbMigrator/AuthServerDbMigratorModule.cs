using LINGYUN.Abp.Data.DbMigrator;
using LINGYUN.Abp.Identity.EntityFrameworkCore;
using LINGYUN.Abp.Saas.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict.EntityFrameworkCore;

namespace LY.MicroService.AuthServer.DbMigrator;

[DependsOn(
    typeof(AbpSaasEntityFrameworkCoreModule),
    typeof(AbpIdentityEntityFrameworkCoreModule),
    typeof(AbpOpenIddictEntityFrameworkCoreModule),
    typeof(AbpDataDbMigratorModule),
    typeof(AbpAutofacModule)
    )]
public partial class AuthServerDbMigratorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        ConfigureDbContext(context.Services);
    }
}

using LINGYUN.Abp.Data.DbMigrator;
using LINGYUN.Abp.DataProtectionManagement.EntityFrameworkCore;
using LINGYUN.Abp.Saas.EntityFrameworkCore;
using LINGYUN.Abp.TextTemplating.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;

namespace LY.MicroService.BackendAdmin.EntityFrameworkCore;

[DependsOn(
    typeof(AbpSaasEntityFrameworkCoreModule),
    typeof(AbpSettingManagementEntityFrameworkCoreModule),
    typeof(AbpDataProtectionManagementEntityFrameworkCoreModule),
    typeof(AbpPermissionManagementEntityFrameworkCoreModule),
    typeof(AbpFeatureManagementEntityFrameworkCoreModule),
    typeof(AbpTextTemplatingEntityFrameworkCoreModule),
    typeof(AbpDataDbMigratorModule)
    )]
public class BackendAdminMigrationsEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<BackendAdminMigrationsDbContext>();

        Configure<AbpDbContextOptions>(options =>
        {
            options.UseMySQL();
        });
    }
}

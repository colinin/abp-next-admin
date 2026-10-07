using LINGYUN.Abp.Data.DbMigrator;
using LINGYUN.Abp.DataProtectionManagement.EntityFrameworkCore;
using LINGYUN.Abp.EntityFrameworkCore.MySQL;
using LINGYUN.Abp.PermissionManagement.EntityFrameworkCore;
using LINGYUN.Abp.Saas.EntityFrameworkCore;
using LINGYUN.Abp.TextTemplating.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement.EntityFrameworkCore;

namespace LINGYUN.Abp.MicroService.AdminService.EntityFrameworkCore;

[DependsOn(
    typeof(AbpSaasEntityFrameworkCoreModule),
    typeof(AbpSettingManagementEntityFrameworkCoreModule),
    typeof(AbpDataProtectionManagementEntityFrameworkCoreModule),
    typeof(AbpPermissionManagementEntityFrameworkCoreModule),
    typeof(AbpFeatureManagementEntityFrameworkCoreModule),
    typeof(AbpTextTemplatingEntityFrameworkCoreModule),
    typeof(AbpEntityFrameworkCoreMySQLMicrotingModule),
    typeof(AbpDataDbMigratorModule)
    )]
public class AdminServiceMigrationsEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<AdminServiceMigrationsDbContext>();

        Configure<AbpDbContextOptions>(options =>
        {
            options.UseMySQL(
                mysql =>
                {
                    // see: https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql/issues/1960
                    mysql.UseParameterizedCollectionMode(ParameterTranslationMode.Constant);
                });
        });
    }
}

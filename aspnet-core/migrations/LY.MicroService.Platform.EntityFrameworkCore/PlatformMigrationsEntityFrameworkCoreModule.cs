using LINGYUN.Abp.BlobManagement.EntityFrameworkCore;
using LINGYUN.Abp.Data.DbMigrator;
using LINGYUN.Abp.EntityFrameworkCore.MySQL;
using LINGYUN.Abp.Saas.EntityFrameworkCore;
using LINGYUN.Abp.UI.Navigation.VueVbenAdmin5;
using LINGYUN.Platform.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;

namespace LY.MicroService.Platform.EntityFrameworkCore;

[DependsOn(
    typeof(AbpUINavigationVueVbenAdmin5Module),
    typeof(AbpSaasEntityFrameworkCoreModule),
    typeof(PlatformEntityFrameworkCoreModule),
    typeof(AbpBlobManagementEntityFrameworkCoreModule),
    typeof(AbpSettingManagementEntityFrameworkCoreModule),
    typeof(AbpPermissionManagementEntityFrameworkCoreModule),
    typeof(AbpFeatureManagementEntityFrameworkCoreModule),
    typeof(AbpEntityFrameworkCoreMySQLMicrotingModule),
    typeof(AbpDataDbMigratorModule)
    )]
public class PlatformMigrationsEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<PlatformMigrationsDbContext>();

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

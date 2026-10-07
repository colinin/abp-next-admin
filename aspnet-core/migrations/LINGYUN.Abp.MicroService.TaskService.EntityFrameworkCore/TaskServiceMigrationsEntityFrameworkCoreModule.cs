using LINGYUN.Abp.Data.DbMigrator;
using LINGYUN.Abp.EntityFrameworkCore.MySQL;
using LINGYUN.Abp.Quartz.MySqlInstaller;
using LINGYUN.Abp.Saas.EntityFrameworkCore;
using LINGYUN.Abp.TaskManagement.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;

namespace LINGYUN.Abp.MicroService.TaskService.EntityFrameworkCore;

[DependsOn(
    typeof(AbpQuartzMySqlInstallerModule),
    typeof(AbpSaasEntityFrameworkCoreModule),
    typeof(TaskManagementEntityFrameworkCoreModule),
    typeof(AbpEntityFrameworkCoreMySQLMicrotingModule),
    typeof(AbpSettingManagementEntityFrameworkCoreModule),
    typeof(AbpPermissionManagementEntityFrameworkCoreModule),
    typeof(AbpFeatureManagementEntityFrameworkCoreModule),
    typeof(AbpDataDbMigratorModule)
    )]
public class TaskServiceMigrationsEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<TaskServiceMigrationsDbContext>();

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

using LINGYUN.Abp.Data.DbMigrator;
using LINGYUN.Abp.Saas.EntityFrameworkCore;
using LINGYUN.Abp.MessageService.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;

namespace LY.MicroService.RealtimeMessage.DbMigrator;

[DependsOn(
    typeof(AbpSaasEntityFrameworkCoreModule),
    typeof(AbpSettingManagementEntityFrameworkCoreModule),
    typeof(AbpPermissionManagementEntityFrameworkCoreModule),
    typeof(AbpFeatureManagementEntityFrameworkCoreModule),
    typeof(AbpMessageServiceEntityFrameworkCoreModule),
    typeof(AbpDataDbMigratorModule),
    typeof(AbpAutofacModule)
    )]
public partial class RealtimeMessageDbMigratorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        ConfigureDbContext(context.Services);
    }
}

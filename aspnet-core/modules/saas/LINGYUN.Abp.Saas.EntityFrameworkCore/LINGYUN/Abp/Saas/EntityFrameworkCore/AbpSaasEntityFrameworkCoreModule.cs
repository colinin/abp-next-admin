using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Saas.EntityFrameworkCore;

[DependsOn(typeof(AbpSaasDomainModule))]
[DependsOn(typeof(AbpEntityFrameworkCoreModule))]
public class AbpSaasEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<SaasDbContext>(options =>
        {
            options.AddDefaultRepositories<ISaasDbContext>();
        });

        Configure<AbpDbConnectionOptions>(options =>
        {
            // 将租户管理连接字符串聚合到 Saas 模块中
            options.Databases.Configure(AbpSaasDbProperties.ConnectionStringName,
                database =>
                {
                    database.MappedConnections.Add("AbpTenantManagement");
                    database.IsUsedByTenants = false;
                });
        });
    }
}

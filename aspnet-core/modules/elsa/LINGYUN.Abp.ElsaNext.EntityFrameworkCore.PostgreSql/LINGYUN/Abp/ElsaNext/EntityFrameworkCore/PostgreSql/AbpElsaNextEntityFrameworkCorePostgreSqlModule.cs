using Elsa.Persistence.EFCore.Extensions;
using Elsa.Persistence.EFCore.Modules.Alterations;
using Elsa.Persistence.EFCore.Modules.Identity;
using Elsa.Persistence.EFCore.Modules.Labels;
using Elsa.Persistence.EFCore.Modules.Management;
using Elsa.Persistence.EFCore.Modules.Runtime;
using Elsa.Persistence.EFCore.Modules.Tenants;
using Microsoft.Extensions.Configuration;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.ElsaNext.EntityFrameworkCore.PostgreSql;

[DependsOn(typeof(AbpElsaNextEntityFrameworkCoreModule))]
public class AbpElsaNextEntityFrameworkCorePostgreSqlModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var connectionString = context.Configuration.GetConnectionString("ElsaNext");

        PreConfigure<EFCoreTenantManagementFeature>(efCore =>
        {
            efCore.UsePostgreSql(connectionString);
        });
        PreConfigure<EFCoreLabelPersistenceFeature>(efCore =>
        {
            efCore.UsePostgreSql(connectionString);
        });
        //PreConfigure<EFCoreIdentityPersistenceFeature>(efCore =>
        //{
        //    efCore.UsePostgreSql(connectionString);
        //});
        PreConfigure<EFCoreAlterationsPersistenceFeature>(efCore =>
        {
            efCore.UsePostgreSql(connectionString);
        });
        PreConfigure<WorkflowManagementPersistenceFeature>(efCore =>
        {
            efCore.UsePostgreSql(connectionString);
        });
        PreConfigure<EFCoreWorkflowRuntimePersistenceFeature>(efCore =>
        {
            efCore.UsePostgreSql(connectionString);
        });
    }
}

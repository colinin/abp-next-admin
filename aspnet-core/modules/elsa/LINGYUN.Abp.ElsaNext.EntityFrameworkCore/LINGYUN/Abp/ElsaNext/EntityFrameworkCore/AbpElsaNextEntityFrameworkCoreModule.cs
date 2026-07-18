using Elsa.Alterations.Extensions;
using Elsa.Extensions;
using Elsa.Features.Services;
using Elsa.Persistence.EFCore.Modules.Alterations;
using Elsa.Persistence.EFCore.Modules.Identity;
using Elsa.Persistence.EFCore.Modules.Labels;
using Elsa.Persistence.EFCore.Modules.Management;
using Elsa.Persistence.EFCore.Modules.Runtime;
using Elsa.Persistence.EFCore.Modules.Tenants;
using Elsa.Tenants.Extensions;
using Elsa.Tenants.Features;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.ElsaNext.EntityFrameworkCore;

[DependsOn(typeof(AbpElsaNextModule))]
public class AbpElsaNextEntityFrameworkCoreModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var labelFeature = context.Services.GetPreConfigureActions<EFCoreLabelPersistenceFeature>();
        var tenantFeature = context.Services.GetPreConfigureActions<EFCoreTenantManagementFeature>();
        var identityFeature = context.Services.GetPreConfigureActions<EFCoreIdentityPersistenceFeature>();
        var alterationsFeature = context.Services.GetPreConfigureActions<EFCoreAlterationsPersistenceFeature>();
        var workflowManagementFeature = context.Services.GetPreConfigureActions<WorkflowManagementPersistenceFeature>();
        var workflowRuntimeFeature = context.Services.GetPreConfigureActions<EFCoreWorkflowRuntimePersistenceFeature>();

        PreConfigure<IModule>(elsa =>
        {
            elsa.UseLabels(label =>
            {
                label.UseEntityFrameworkCore(efCore =>
                {
                    labelFeature.Configure(efCore);
                });
            });
            //elsa.UseIdentity(identity =>
            //{
            //    identity.UseEntityFrameworkCore(efCore =>
            //    {
            //        identityFeature.Configure(efCore);
            //    });
            //});
            elsa.UseAlterations(alterations =>
            {
                alterations.UseEntityFrameworkCore(efCore =>
                {
                    alterationsFeature.Configure(efCore);
                });
            });
            elsa.UseWorkflowManagement(management =>
            {
                management.UseEntityFrameworkCore(efCore =>
                {
                    workflowManagementFeature.Configure(efCore);
                });
            });
            elsa.UseWorkflowRuntime(runtime =>
            {
                runtime.UseEntityFrameworkCore(efCore =>
                {
                    workflowRuntimeFeature.Configure(efCore);
                });
            });
            elsa.UseTenants(tenant =>
            {
                tenant.UseTenantManagement(tenantManagement =>
                {
                    tenantManagement.UseEntityFrameworkCore(efCore =>
                    {
                        tenantFeature.Configure(efCore);
                    });
                });
            });
        });
    }
}

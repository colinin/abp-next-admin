using Elsa.Agents;
using Elsa.Agents.Persistence.EFCore;
using Elsa.AI.Persistence.EFCore.Features;
using Elsa.Alterations.Extensions;
using Elsa.Extensions;
using Elsa.Features.Services;
using Elsa.Persistence.EFCore.Modules.Alterations;
using Elsa.Persistence.EFCore.Modules.Labels;
using Elsa.Persistence.EFCore.Modules.Management;
using Elsa.Persistence.EFCore.Modules.Runtime;
using Elsa.Secrets.Persistence.EFCore.Extensions;
using Elsa.Secrets.Persistence.EFCore.Features;
using LINGYUN.Abp.ElsaNext.EntityFrameworkCore.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.ElsaNext.EntityFrameworkCore;

public partial class AbpElsaNextEntityFrameworkCoreModule
{
    private void PreConfigureWorkflowManagement(ServiceConfigurationContext context)
    {
        if (context.IsElsaFeatureEnabled("WorkflowManagement:EfCore"))
        {
            var featureConfig = context.Services.GetPreConfigureActions<WorkflowManagementPersistenceFeature>();
            PreConfigure<IModule>(elsa =>
            {
                elsa.UseWorkflowManagement(feature =>
                {
                    feature.UseEntityFrameworkCore(efCore =>
                    {
                        featureConfig.Configure(efCore);
                    });
                });
            });
        }
    }

    private void PreConfigureWorkflowRuntime(ServiceConfigurationContext context)
    {
        if (context.IsElsaFeatureEnabled("WorkflowRuntime:EfCore"))
        {
            var featureConfig = context.Services.GetPreConfigureActions<EFCoreWorkflowRuntimePersistenceFeature>();
            PreConfigure<IModule>(elsa =>
            {
                elsa.UseWorkflowRuntime(feature =>
                {
                    feature.UseEntityFrameworkCore(efCore =>
                    {
                        featureConfig.Configure(efCore);
                    });
                });
            });
        }
    }

    private void PreConfigureAlterations(ServiceConfigurationContext context)
    {
        if (context.IsElsaFeatureEnabled("Alterations:EfCore"))
        {
            var featureConfig = context.Services.GetPreConfigureActions<EFCoreAlterationsPersistenceFeature>();
            PreConfigure<IModule>(elsa =>
            {
                elsa.UseAlterations(feature =>
                {
                    feature.UseEntityFrameworkCore(efCore =>
                    {
                        featureConfig.Configure(efCore);
                    });
                });
            });
        }
    }

    private void PreConfigureAgent(ServiceConfigurationContext context)
    {
        if (context.IsElsaFeatureEnabled("Agent:EfCore"))
        {
            var featureConfig = context.Services.GetPreConfigureActions<EFCoreAgentPersistenceFeature>();
            PreConfigure<IModule>(elsa =>
            {
                elsa.UseAgentPersistence(feature =>
                {
                    feature.UseEntityFrameworkCore(efCore =>
                    {
                        featureConfig.Configure(efCore);
                    });
                });
            });
        }
    }

    private void PreConfigureLabels(ServiceConfigurationContext context)
    {
        if (context.IsElsaFeatureEnabled("Labels:EfCore"))
        {
            var featureConfig = context.Services.GetPreConfigureActions<EFCoreLabelPersistenceFeature>();
            PreConfigure<IModule>(elsa =>
            {
                elsa.UseLabels(feature =>
                {
                    feature.UseEntityFrameworkCore(efCore =>
                    {
                        featureConfig.Configure(efCore);
                    });
                });
            });
        }
    }

    private void PreConfigureSecrets(ServiceConfigurationContext context)
    {
        if (context.IsElsaFeatureEnabled("Secrets:EfCore"))
        {
            var featureConfig = context.Services.GetPreConfigureActions<EFCoreSecretsPersistenceFeature>();
            PreConfigure<IModule>(elsa =>
            {
                elsa.UseSecrets(feature =>
                {
                    feature.UseEntityFrameworkCore(efCore =>
                    {
                        featureConfig.Configure(efCore);
                    });
                });
            });
        }
    }

    private void PreConfigureWeaver(ServiceConfigurationContext context)
    {
        if (context.IsElsaFeatureEnabled("Weaver:EfCore"))
        {
            var featureConfig = context.Services.GetPreConfigureActions<AIPersistenceFeature>();
            PreConfigure<IModule>(elsa =>
                elsa.Configure<AIPersistenceFeature>(feature => featureConfig.Configure(feature)));
        }
    }
}

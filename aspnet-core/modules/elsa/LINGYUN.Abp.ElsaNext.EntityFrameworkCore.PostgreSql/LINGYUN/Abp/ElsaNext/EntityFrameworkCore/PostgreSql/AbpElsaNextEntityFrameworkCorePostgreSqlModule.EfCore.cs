using Elsa.Agents.Persistence.EFCore;
using Elsa.AI.Persistence.EFCore;
using Elsa.AI.Persistence.EFCore.Features;
using Elsa.AI.Persistence.EFCore.PostgreSql.Extensions;
using Elsa.Extensions;
using Elsa.Features.Services;
using Elsa.Persistence.EFCore;
using Elsa.Persistence.EFCore.Extensions;
using Elsa.Persistence.EFCore.Modules.Alterations;
using Elsa.Persistence.EFCore.Modules.Labels;
using Elsa.Persistence.EFCore.Modules.Management;
using Elsa.Persistence.EFCore.Modules.Runtime;
using Elsa.Scheduling.Quartz.Features;
using Elsa.Secrets.Persistence.EFCore.Features;
using Elsa.Secrets.Persistence.EFCore.PostgreSql.Extensions;
using LINGYUN.Abp.ElsaNext.EntityFrameworkCore.AI;
using LINGYUN.Abp.ElsaNext.EntityFrameworkCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.ElsaNext.EntityFrameworkCore.PostgreSql;

public partial class AbpElsaNextEntityFrameworkCorePostgreSqlModule
{
    private const string DefaultConnectionString = "Host=127.0.0.1;Database=elsa;Username=postgres;Password=123456;";
    private void PreConfigureWorkflowManagement(ServiceConfigurationContext context)
    {
        if (context.IsElsaFeatureEnabled("WorkflowManagement:EfCore:PostgreSql"))
        {
            PreConfigure<WorkflowManagementPersistenceFeature>(efCore =>
            {
                efCore.UsePostgreSql(sp =>
                {
                    var configuration = sp.GetRequiredService<IConfiguration>();
                    return
                        configuration["Elsa:WorkflowManagement:EfCore:PostgreSql:ConnectionString"]
                        ?? configuration.GetConnectionStringOrDefault("Workflow")
                        ?? DefaultConnectionString;
                });
            });
        }
    }

    private void PreConfigureWorkflowRuntime(ServiceConfigurationContext context)
    {
        if (context.IsElsaFeatureEnabled("WorkflowRuntime:EfCore:PostgreSql"))
        {
            PreConfigure<EFCoreWorkflowRuntimePersistenceFeature>(efCore =>
            {
                efCore.UsePostgreSql(sp =>
                {
                    var configuration = sp.GetRequiredService<IConfiguration>();
                    return
                        configuration["Elsa:WorkflowRuntime:EfCore:PostgreSql:ConnectionString"]
                        ?? configuration.GetConnectionStringOrDefault("Workflow")
                        ?? DefaultConnectionString;
                });
            });
        }
    }

    private void PreConfigureAlterations(ServiceConfigurationContext context)
    {
        if (context.IsElsaFeatureEnabled("Alterations:EfCore:PostgreSql"))
        {
            PreConfigure<EFCoreAlterationsPersistenceFeature>(efCore =>
            {
                efCore.UsePostgreSql(sp =>
                {
                    var configuration = sp.GetRequiredService<IConfiguration>();
                    return
                        configuration["Elsa:Alterations:EfCore:PostgreSql:ConnectionString"]
                        ?? configuration.GetConnectionStringOrDefault("Workflow")
                        ?? DefaultConnectionString;
                });
            });
        }
    }

    private void PreConfigureAgent(ServiceConfigurationContext context)
    {
        if (context.IsElsaFeatureEnabled("Agent:EfCore:PostgreSql"))
        {
            PreConfigure<EFCoreAgentPersistenceFeature>(efCore =>
            {
                // Agents 的迁移位于 Elsa.Agents.Persistence.EFCore.PostgreSql，传入其它程序集时 EF 找不到待应用迁移
                efCore.UsePostgreSql(typeof(AgentsPostgreSqlProvidersExtensions).Assembly, sp =>
                {
                    var configuration = sp.GetRequiredService<IConfiguration>();
                    return
                        configuration["Elsa:Agent:EfCore:PostgreSql:ConnectionString"]
                        ?? configuration.GetConnectionStringOrDefault("Workflow")
                        ?? DefaultConnectionString;
                });
            });
        }
    }

    private void PreConfigureLabels(ServiceConfigurationContext context)
    {
        if (context.IsElsaFeatureEnabled("Labels:EfCore:PostgreSql"))
        {
            PreConfigure<EFCoreLabelPersistenceFeature>(efCore =>
            {
                efCore.UsePostgreSql(sp =>
                {
                    var configuration = sp.GetRequiredService<IConfiguration>();
                    return
                        configuration["Elsa:Labels:EfCore:PostgreSql:ConnectionString"]
                        ?? configuration.GetConnectionStringOrDefault("Workflow")
                        ?? DefaultConnectionString;
                });
            });
        }
    }

    private void PreConfigureSecrets(ServiceConfigurationContext context)
    {
        if (context.IsElsaFeatureEnabled("Secrets:EfCore:PostgreSql"))
        {
            PreConfigure<EFCoreSecretsPersistenceFeature>(efCore =>
            {
                efCore.UsePostgreSql(sp =>
                {
                    var configuration = sp.GetRequiredService<IConfiguration>();
                    return
                        configuration["Elsa:Secrets:EfCore:PostgreSql:ConnectionString"]
                        ?? configuration.GetConnectionStringOrDefault("Workflow")
                        ?? DefaultConnectionString;
                });
            });
        }
    }

    private void PreConfigureWeaver(ServiceConfigurationContext context)
    {
        if (context.IsElsaFeatureEnabled("Weaver:EfCore:PostgreSql"))
        {
            var connectionString = context.Configuration["Elsa:Weaver:EfCore:PostgreSql:ConnectionString"]
                ?? context.Configuration.GetConnectionStringOrDefault("Workflow")
                ?? DefaultConnectionString;

            PreConfigure<AIPersistenceFeature>(feature => feature.UsePostgreSql(connectionString));

            context.Services.AddDbContextFactory<AIDbContext, AbpAIDbContextFactory>(
                builder => builder.UseElsaPostgreSql(
                    typeof(PostgreSqlAIPersistenceFeatureExtensions).Assembly,
                    connectionString),
                ServiceLifetime.Scoped);

            context.Services.Decorate<IDbContextFactory<AIDbContext>, TenantAwareDbContextFactory<AIDbContext>>();
            context.Services.AddAIPersistenceStores();

            //PreConfigure<AIPersistenceFeature>(efCore =>
            //{
            //    efCore.UsePostgreSql(sp =>
            //    {
            //        var configuration = sp.GetRequiredService<IConfiguration>();
            //        return
            //            configuration["Elsa:Weaver:EfCore:PostgreSql:ConnectionString"]
            //            ?? configuration.GetConnectionStringOrDefault("Workflow")
            //            ?? DefaultConnectionString;
            //    });
            //});
        }
    }

    private void PreConfigureQuartz(ServiceConfigurationContext context)
    {
        if (context.IsElsaFeatureEnabled("Quartz"))
        {
            var quartzFeature = context.Services.GetPreConfigureActions<QuartzFeature>();
            var quartzSchedulerFeature = context.Services.GetPreConfigureActions<QuartzSchedulerFeature>();
            PreConfigure<IModule>(elsa =>
            {
                elsa.UseQuartz(quartz =>
                {
                    if (context.IsElsaFeatureEnabled("Quartz:EfCore:PostgreSql"))
                    {
                        quartz.UsePostgreSql(
                            context.Configuration["Elsa:Quartz:EfCore:PostgreSql:ConnectionString"]
                            ?? context.Configuration.GetConnectionStringOrDefault("Workflow")
                            ?? DefaultConnectionString);
                    }

                    var quartzProperties = context.Configuration.GetSection("Elsa:Quartz:Properties").Get<Dictionary<string, string?>>();
                    if (quartzProperties != null)
                    {
                        quartz.ConfigureQuartzOptions += options =>
                        {
                            foreach (var quartzProperty in quartzProperties)
                            {
                                options[quartzProperty.Key] = quartzProperty.Value;
                            }
                        };
                    }

                    var instanceId = context.Configuration["Elsa:Quartz:Properties:InstanceId"] ?? "AUTO";
                    var schedulerName = context.Configuration["Elsa:Quartz:Properties:SchedulerName"] ?? "ElsaScheduler";
                    quartz.ConfigureClusteringIdentity(instanceId, schedulerName);

                    quartzFeature.Configure(quartz);
                });

                elsa.UseScheduling(scheduling =>
                {
                    scheduling.UseQuartzScheduler(quartz =>
                    {
                        quartzSchedulerFeature.Configure(quartz);
                    });
                });
            });
        }
    }
}

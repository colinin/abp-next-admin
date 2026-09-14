using Elsa.Agents.Persistence.EFCore;
using Elsa.AI.Persistence.EFCore;
using Elsa.AI.Persistence.EFCore.Features;
using Elsa.AI.Persistence.EFCore.Sqlite.Extensions;
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
using Elsa.Secrets.Persistence.EFCore.Sqlite.Extensions;
using LINGYUN.Abp.ElsaNext.EntityFrameworkCore.AI;
using LINGYUN.Abp.ElsaNext.EntityFrameworkCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.ElsaNext.EntityFrameworkCore.Sqlite;

public partial class AbpElsaNextEntityFrameworkCoreSqliteModule
{
    private const string DefaultConnectionString = "Data Source=elsa.sqlite.db;Cache=Shared;";
    private void PreConfigureWorkflowManagement(ServiceConfigurationContext context)
    {
        if (context.IsElsaFeatureEnabled("WorkflowManagement:EfCore:Sqlite"))
        {
            PreConfigure<WorkflowManagementPersistenceFeature>(efCore =>
            {
                efCore.UseSqlite(sp =>
                {
                    var configuration = sp.GetRequiredService<IConfiguration>();
                    return
                        configuration["Elsa:WorkflowManagement:EfCore:Sqlite:ConnectionString"]
                        ?? configuration.GetConnectionStringOrDefault("Workflow")
                        ?? DefaultConnectionString;
                });
            });
        }
    }

    private void PreConfigureWorkflowRuntime(ServiceConfigurationContext context)
    {
        if (context.IsElsaFeatureEnabled("WorkflowRuntime:EfCore:Sqlite"))
        {
            PreConfigure<EFCoreWorkflowRuntimePersistenceFeature>(efCore =>
            {
                efCore.UseSqlite(sp =>
                {
                    var configuration = sp.GetRequiredService<IConfiguration>();
                    return
                        configuration["Elsa:WorkflowRuntime:EfCore:Sqlite:ConnectionString"]
                        ?? configuration.GetConnectionStringOrDefault("Workflow")
                        ?? DefaultConnectionString;
                });
            });
        }
    }

    private void PreConfigureAlterations(ServiceConfigurationContext context)
    {
        if (context.IsElsaFeatureEnabled("Alterations:EfCore:Sqlite"))
        {
            PreConfigure<EFCoreAlterationsPersistenceFeature>(efCore =>
            {
                efCore.UseSqlite(sp =>
                {
                    var configuration = sp.GetRequiredService<IConfiguration>();
                    return
                        configuration["Elsa:Alterations:EfCore:Sqlite:ConnectionString"]
                        ?? configuration.GetConnectionStringOrDefault("Workflow")
                        ?? DefaultConnectionString;
                });
            });
        }
    }

    private void PreConfigureAgent(ServiceConfigurationContext context)
    {
        if (context.IsElsaFeatureEnabled("Agent:EfCore:Sqlite"))
        {
            PreConfigure<EFCoreAgentPersistenceFeature>(efCore =>
            {
                // Agents 的迁移位于 Elsa.Agents.Persistence.EFCore.Sqlite，传入其它程序集时 EF 找不到待应用迁移
                efCore.UseSqlite(typeof(AgentsSqliteProvidersExtensions).Assembly, sp =>
                {
                    var configuration = sp.GetRequiredService<IConfiguration>();
                    return
                        configuration["Elsa:Agent:EfCore:Sqlite:ConnectionString"]
                        ?? configuration.GetConnectionStringOrDefault("Workflow")
                        ?? DefaultConnectionString;
                });
            });
        }
    }

    private void PreConfigureLabels(ServiceConfigurationContext context)
    {
        if (context.IsElsaFeatureEnabled("Labels:EfCore:Sqlite"))
        {
            PreConfigure<EFCoreLabelPersistenceFeature>(efCore =>
            {
                efCore.UseSqlite(sp =>
                {
                    var configuration = sp.GetRequiredService<IConfiguration>();
                    return
                        configuration["Elsa:Labels:EfCore:Sqlite:ConnectionString"]
                        ?? configuration.GetConnectionStringOrDefault("Workflow")
                        ?? DefaultConnectionString;
                });
            });
        }
    }

    private void PreConfigureSecrets(ServiceConfigurationContext context)
    {
        if (context.IsElsaFeatureEnabled("Secrets:EfCore:Sqlite"))
        {
            PreConfigure<EFCoreSecretsPersistenceFeature>(efCore =>
            {
                efCore.UseSqlite(sp =>
                {
                    var configuration = sp.GetRequiredService<IConfiguration>();
                    return
                        configuration["Elsa:Secrets:EfCore:Sqlite:ConnectionString"]
                        ?? configuration.GetConnectionStringOrDefault("Workflow")
                        ?? DefaultConnectionString;
                });
            });
        }
    }

    private void PreConfigureWeaver(ServiceConfigurationContext context)
    {
        if (context.IsElsaFeatureEnabled("Weaver:EfCore:Sqlite"))
        {
            var connectionString = context.Configuration["Elsa:Weaver:EfCore:Sqlite:ConnectionString"]
                ?? context.Configuration.GetConnectionStringOrDefault("Workflow")
                ?? DefaultConnectionString;

            PreConfigure<AIPersistenceFeature>(feature => feature.UseSqlite(connectionString));

            context.Services.AddDbContextFactory<AIDbContext, AbpAIDbContextFactory>(
                builder => builder.UseElsaSqlite(
                    typeof(SqliteAIPersistenceFeatureExtensions).Assembly,
                    connectionString),
                ServiceLifetime.Scoped);

            context.Services.Decorate<IDbContextFactory<AIDbContext>, TenantAwareDbContextFactory<AIDbContext>>();
            context.Services.Configure<MigrationOptions>(o => o.RunMigrations[typeof(AIDbContext)] = true);
            context.Services.AddStartupTask<RunMigrationsStartupTask<AIDbContext>>();
            context.Services.AddAIPersistenceStores();
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
                    if (context.IsElsaFeatureEnabled("Quartz:EfCore:Sqlite"))
                    {
                        quartz.UseSqlite(
                            context.Configuration["Elsa:Quartz:EfCore:Sqlite:ConnectionString"]
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

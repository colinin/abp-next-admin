using Elsa.Agents.Persistence.EFCore;
using Elsa.AI.Persistence.EFCore;
using Elsa.AI.Persistence.EFCore.Features;
using Elsa.AI.Persistence.EFCore.MySql.Extensions;
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
using Elsa.Secrets.Persistence.EFCore.MySql.Extensions;
using LINGYUN.Abp.ElsaNext.EntityFrameworkCore.AI;
using LINGYUN.Abp.ElsaNext.EntityFrameworkCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.ElsaNext.EntityFrameworkCore.MySQL;

public partial class AbpElsaNextEntityFrameworkCoreMySQLModule
{
    private const string DefaultConnectionString = "Server=127.0.0.1;Database=elsa;User Id=root;Password=123456;";
    private void PreConfigureWorkflowManagement(ServiceConfigurationContext context)
    {
        if (context.IsElsaFeatureEnabled("WorkflowManagement:EfCore:MySQL"))
        {
            PreConfigure<WorkflowManagementPersistenceFeature>(efCore =>
            {
                efCore.UseMySql(sp =>
                {
                    var configuration = sp.GetRequiredService<IConfiguration>();
                    return
                        configuration["Elsa:WorkflowManagement:EfCore:MySQL:ConnectionString"] 
                        ?? configuration.GetConnectionStringOrDefault("Workflow") 
                        ?? DefaultConnectionString;
                });
            });
        }
    }

    private void PreConfigureWorkflowRuntime(ServiceConfigurationContext context)
    {
        if (context.IsElsaFeatureEnabled("WorkflowRuntime:EfCore:MySQL"))
        {
            PreConfigure<EFCoreWorkflowRuntimePersistenceFeature>(efCore =>
            {
                efCore.UseMySql(sp =>
                {
                    var configuration = sp.GetRequiredService<IConfiguration>();
                    return
                        configuration["Elsa:WorkflowRuntime:EfCore:MySQL:ConnectionString"]
                        ?? configuration.GetConnectionStringOrDefault("Workflow")
                        ?? DefaultConnectionString;
                });
            });
        }
    }

    private void PreConfigureAlterations(ServiceConfigurationContext context)
    {
        if (context.IsElsaFeatureEnabled("Alterations:EfCore:MySQL"))
        {
            PreConfigure<EFCoreAlterationsPersistenceFeature>(efCore =>
            {
                efCore.UseMySql(sp =>
                {
                    var configuration = sp.GetRequiredService<IConfiguration>();
                    return
                        configuration["Elsa:Alterations:EfCore:MySQL:ConnectionString"]
                        ?? configuration.GetConnectionStringOrDefault("Workflow")
                        ?? DefaultConnectionString;
                });
            });
        }
    }

    private void PreConfigureAgent(ServiceConfigurationContext context)
    {
        if (context.IsElsaFeatureEnabled("Agent:EfCore:MySQL"))
        {
            PreConfigure<EFCoreAgentPersistenceFeature>(efCore =>
            {
                // TODO: Agents 的迁移位于 Elsa.Agents.Persistence.EFCore.MySql，正式启用 MySQL 前需要像
                // Elsa.AI.Persistence.EFCore.MySql 一样把该包的源码 vendor 到本项目，再在此指定迁移程序集。
                // 直接引用该包会连同上游 Elsa.Persistence.EFCore.MySql 一起引入，与本地 vendor 的同名类型冲突。
                efCore.UseMySql(sp =>
                {
                    var configuration = sp.GetRequiredService<IConfiguration>();
                    return
                        configuration["Elsa:Agent:EfCore:MySQL:ConnectionString"]
                        ?? configuration.GetConnectionStringOrDefault("Workflow")
                        ?? DefaultConnectionString;
                });
            });
        }
    }

    private void PreConfigureLabels(ServiceConfigurationContext context)
    {
        if (context.IsElsaFeatureEnabled("Labels:EfCore:MySQL"))
        {
            PreConfigure<EFCoreLabelPersistenceFeature>(efCore =>
            {
                efCore.UseMySql(sp =>
                {
                    var configuration = sp.GetRequiredService<IConfiguration>();
                    return
                        configuration["Elsa:Labels:EfCore:MySQL:ConnectionString"]
                        ?? configuration.GetConnectionStringOrDefault("Workflow")
                        ?? DefaultConnectionString;
                });
            });
        }
    }

    private void PreConfigureSecrets(ServiceConfigurationContext context)
    {
        if (context.IsElsaFeatureEnabled("Secrets:EfCore:MySQL"))
        {
            PreConfigure<EFCoreSecretsPersistenceFeature>(efCore =>
            {
                efCore.UseMySql(sp =>
                {
                    var configuration = sp.GetRequiredService<IConfiguration>();
                    return
                        configuration["Elsa:Secrets:EfCore:MySQL:ConnectionString"]
                        ?? configuration.GetConnectionStringOrDefault("Workflow")
                        ?? DefaultConnectionString;
                });
            });
        }
    }

    private void PreConfigureWeaver(ServiceConfigurationContext context)
    {
        if (context.IsElsaFeatureEnabled("Weaver:EfCore:MySQL"))
        {
            var connectionString = context.Configuration["Elsa:Weaver:EfCore:MySQL:ConnectionString"]
                ?? context.Configuration.GetConnectionStringOrDefault("Workflow")
                ?? DefaultConnectionString;

            PreConfigure<AIPersistenceFeature>(feature => feature.UseMySql(connectionString));

            context.Services.AddDbContextFactory<AIDbContext, AbpAIDbContextFactory>(
                builder => builder.UseElsaMySql(
                    typeof(MySqlAIPersistenceFeatureExtensions).Assembly,
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
                    if (context.IsElsaFeatureEnabled("Quartz:EfCore:MySQL"))
                    {
                        quartz.UseMySql(
                            context.Configuration["Elsa:Quartz:EfCore:MySQL:ConnectionString"]
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

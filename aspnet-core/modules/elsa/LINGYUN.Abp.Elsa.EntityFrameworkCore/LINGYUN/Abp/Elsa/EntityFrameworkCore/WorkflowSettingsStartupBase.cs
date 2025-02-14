using Elsa.Options;
using Elsa.WorkflowSettings;
using Elsa.WorkflowSettings.Extensions;
using Elsa.WorkflowSettings.Persistence.EntityFramework.Core;
using Elsa.WorkflowSettings.Persistence.EntityFramework.Core.Extensions;
using Elsa.WorkflowSettings.Persistence.EntityFramework.Core.Stores;
using LINGYUN.Abp.Elsa.EntityFrameworkCore.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace LINGYUN.Abp.Elsa.EntityFrameworkCore;

public abstract class WorkflowSettingsStartupBase : EntityFrameworkWorkflowSettingsStartupBase
{
    public override void ConfigureElsa(ElsaOptionsBuilder elsa, IConfiguration configuration)
    {
        var section = configuration.GetSection("Elsa:Features:WorkflowSettings");
        var connectionStringName = section.GetValue<string>("ConnectionStringIdentifier");
        var connectionString = section.GetValue<string>("ConnectionString");
        if (connectionString.IsNullOrWhiteSpace())
        {
            if (connectionStringName.IsNullOrWhiteSpace())
            {
                connectionStringName = ProviderName;
            }

            connectionString = configuration.GetConnectionString(connectionStringName);
        }

        if (connectionString.IsNullOrWhiteSpace())
        {
            connectionString = GetDefaultConnectionString();
        }

        var workflowSettingsOptionsBuilder = new WorkflowSettingsOptionsBuilder(elsa.Services);
        workflowSettingsOptionsBuilder.UseEntityFrameworkPersistence(options =>
        {
            Configure(options, connectionString);
        }, autoRunMigrations: false);
        elsa.Services.AddScoped((provider) => workflowSettingsOptionsBuilder.WorkflowSettingsOptions.WorkflowSettingsStoreFactory(provider));
        elsa.AddWorkflowSettings();

        elsa.Services.Replace<EntityFrameworkWorkflowSettingsStore, FixedEntityFrameworkWorkflowSettingsStore>(ServiceLifetime.Scoped);
    }
}

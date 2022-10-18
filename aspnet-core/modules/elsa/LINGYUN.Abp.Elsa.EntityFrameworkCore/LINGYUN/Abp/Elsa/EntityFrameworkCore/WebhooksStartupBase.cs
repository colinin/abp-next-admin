using Elsa.Activities.Webhooks;
using Elsa.Options;
using Elsa.Webhooks.Persistence.EntityFramework.Core;
using Elsa.Webhooks.Persistence.EntityFramework.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace LINGYUN.Abp.Elsa.EntityFrameworkCore;

public abstract class WebhooksStartupBase : EntityFrameworkWebhookStartupBase
{
    public override void ConfigureElsa(ElsaOptionsBuilder elsa, IConfiguration configuration)
    {
        var section = configuration.GetSection("Elsa:Features:Webhooks");
        var connectionStringName = section.GetValue<string>("ConnectionStringIdentifier");
        var connectionString = section.GetValue<string>("ConnectionString");
        if (connectionStringName.IsNullOrWhiteSpace())
        {
            connectionStringName = ProviderName;
        }

        if (connectionString.IsNullOrWhiteSpace())
        {
            connectionString = configuration.GetConnectionString(connectionStringName);
        }

        if (connectionString.IsNullOrWhiteSpace())
        {
            connectionString = GetDefaultConnectionString();
        }

        var webhookOptionsBuilder = new WebhookOptionsBuilder(elsa.Services);
        webhookOptionsBuilder.UseEntityFrameworkPersistence(options =>
        {
            Configure(options, connectionString);
        }, autoRunMigrations: false);
        elsa.Services.AddSingleton(webhookOptionsBuilder.WebhookOptions);
    }
}

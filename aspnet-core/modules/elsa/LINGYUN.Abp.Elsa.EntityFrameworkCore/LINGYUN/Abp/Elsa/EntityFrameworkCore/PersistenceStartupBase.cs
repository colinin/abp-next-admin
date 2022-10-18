using Elsa.Options;
using Elsa.Persistence.EntityFramework.Core;
using Elsa.Persistence.EntityFramework.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace LINGYUN.Abp.Elsa.EntityFrameworkCore;

public abstract class PersistenceStartupBase : EntityFrameworkCoreStartupBase
{
    public override void ConfigureElsa(ElsaOptionsBuilder elsa, IConfiguration configuration)
    {
        var section = configuration.GetSection("Elsa:Features:DefaultPersistence");
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

        elsa.UseEntityFrameworkPersistence(options =>
        {
            Configure(options, connectionString);
        }, autoRunMigrations: false);
    }
}

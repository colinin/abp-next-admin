using Elsa.Attributes;
using Elsa.Persistence.EntityFramework.MySql;
using Microsoft.EntityFrameworkCore;

namespace LINGYUN.Abp.Elsa.EntityFrameworkCore.MySql;

[Feature("DefaultPersistence:EntityFrameworkCore:MySql")]
public class PersistenceStartup : PersistenceStartupBase
{
    protected override string ProviderName => "MySql";

    protected override void Configure(DbContextOptionsBuilder options, string connectionString)
    {
#pragma warning disable CS0618
        options.UseMySql(
            connectionString,
            ServerVersion.AutoDetect(connectionString),
            mysql => mysql.TranslateParameterizedCollectionsToConstants());
#pragma warning restore CS0618
    }
}

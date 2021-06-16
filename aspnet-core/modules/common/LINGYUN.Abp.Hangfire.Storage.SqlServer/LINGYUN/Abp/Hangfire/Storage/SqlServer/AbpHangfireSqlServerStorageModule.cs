using Hangfire;
using Hangfire.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Hangfire;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Hangfire.Storage.SqlServer
{
    [DependsOn(typeof(AbpHangfireModule))]
    public class AbpHangfireSqlServerStorageModule : AbpModule
    {
        private SqlServerStorage _jobStorage;

        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            var sqlserverStorageOptions = new SqlServerStorageOptions();
            configuration.GetSection("Hangfire:SqlServer").Bind(sqlserverStorageOptions);

            var hangfireSqlServerConfiguration = configuration.GetSection("Hangfire:SqlServer:Connection");
            var hangfireSqlServerCon = hangfireSqlServerConfiguration.Exists()
                    ? hangfireSqlServerConfiguration.Value : configuration.GetConnectionString("Default");

            _jobStorage = new SqlServerStorage(hangfireSqlServerCon, sqlserverStorageOptions);
            context.Services.AddSingleton<JobStorage, SqlServerStorage>(fac =>
            {
                return _jobStorage;
            });

            PreConfigure<IGlobalConfiguration>(config =>
            {
                config.UseStorage(_jobStorage);
            });
        }
    }
}

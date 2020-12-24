using Hangfire;
using Hangfire.MySql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Hangfire;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Hangfire.Storage.MySql
{
    [DependsOn(typeof(AbpHangfireModule))]
    public class AbpHangfireMySqlStorageModule : AbpModule
    {
        private MySqlStorage _jobStorage;

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            var mysqlStorageOptions = new MySqlStorageOptions();
            configuration.GetSection("Hangfire:MySql").Bind(mysqlStorageOptions);

            var hangfireMySqlConfiguration = configuration.GetSection("Hangfire:MySql:Connection");
            var hangfireMySqlCon = hangfireMySqlConfiguration.Exists()
                    ? hangfireMySqlConfiguration.Value : configuration.GetConnectionString("Default");

            _jobStorage = new MySqlStorage(hangfireMySqlCon, mysqlStorageOptions);
            context.Services.AddSingleton<JobStorage, MySqlStorage>(fac =>
            {
                return _jobStorage;
            });

            context.Services.AddHangfire(config =>
            {
                config.UseStorage(_jobStorage);
            });
        }

        public override void OnApplicationShutdown(ApplicationShutdownContext context)
        {
            _jobStorage?.Dispose();
        }
    }
}

using LINGYUN.Abp.Tests;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;
using Volo.Abp.Uow;

namespace LINGYUN.Abp.EntityFrameworkCore.Tests
{
    [DependsOn(
        typeof(AbpTestsBaseModule)
        )]
    public class AbpEntityFrameworkCoreTestModule : AbpModule
    {
        //private string _testDbFile = "./abp-ef-test-db.db";
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddEntityFrameworkInMemoryDatabase();
            //var connectionString = $"Data Source={_testDbFile}";

            //var sqliteConnection = CreateDatabaseAndGetConnection(connectionString);

            var memoryDbName = Guid.NewGuid().ToString();

            var dbConetxt = CreateDatabaseAndGetDbContext(memoryDbName);

            AsyncHelper.RunSync(async () =>
                await new EfCoreTestEntityDataSeeder(dbConetxt).SeedAsync());

            context.Services.AddSingleton(dbConetxt);

            var databaseName = Guid.NewGuid().ToString();

            Configure<AbpDbContextOptions>(options =>
            {
                options.Configure(abpDbContextConfigurationContext =>
                {
                    abpDbContextConfigurationContext.DbContextOptions.EnableDetailedErrors();
                    abpDbContextConfigurationContext.DbContextOptions.EnableSensitiveDataLogging();
                    abpDbContextConfigurationContext.DbContextOptions.UseEFCoreLogger();

                    abpDbContextConfigurationContext.DbContextOptions.UseInMemoryDatabase(memoryDbName);
                });
            });

            Configure<AbpUnitOfWorkDefaultOptions>(options =>
            {
                options.TransactionBehavior = UnitOfWorkTransactionBehavior.Disabled; //EF in-memory database does not support transactions
            });
        }

        private EfCoreTestDbContext CreateDatabaseAndGetDbContext(string dbName)
        {
            return new EfCoreTestDbContext(
                new DbContextOptionsBuilder<EfCoreTestDbContext>().UseInMemoryDatabase(dbName).Options
            );
        }
    }
}

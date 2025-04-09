using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PackageName.CompanyName.ProjectName.AIO.EntityFrameworkCore;
using System;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;
using Volo.Abp.Timing;
using Volo.Abp.Uow;

namespace PackageName.CompanyName.ProjectName.EntityFrameworkCore;

[DependsOn(
    typeof(ProjectNameTestBaseModule),
    typeof(ProjectNameEntityFrameworkCoreModule),
    typeof(SingleMigrationsEntityFrameworkCoreModule)
)]
public class ProjectNameEntityFrameworkCoreTestModule : AbpModule
{
    // 数据库配置
    private const string DefaultPostgresConnectionString =
        "Host=127.0.0.1;Port=5432;Database=test_db;User Id=postgres;Password=postgres;";

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var connectionString = Environment.GetEnvironmentVariable("TEST_CONNECTION_STRING") ??
                               DefaultPostgresConnectionString;

        // 配置数据库连接字符串
        Configure<AbpDbConnectionOptions>(options =>
        {
            options.ConnectionStrings.Default = connectionString;
        });

        Configure<AbpClockOptions>(options => { options.Kind = DateTimeKind.Utc; });
        context.Services.AddAbpDbContext<SingleMigrationsDbContext>(options =>
        {
            options.AddDefaultRepositories(true);
        });

        // 配置所有DbContext
        Configure<AbpDbContextOptions>(options =>
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            options.UseNpgsql();
        });

        Configure<AbpUnitOfWorkDefaultOptions>(options =>
        {
            options.TransactionBehavior = UnitOfWorkTransactionBehavior.Disabled;
        });
    }

    public override void OnPreApplicationInitialization(ApplicationInitializationContext context)
    {
        var dbContext = context.ServiceProvider.GetRequiredService<SingleMigrationsDbContext>();
        // 重置数据库
        dbContext.Database.EnsureDeleted();
        // // 创建数据库
        dbContext.Database.EnsureCreated();
        dbContext.Database.GenerateCreateScript();
        // dbContext.Database.Migrate();

        // 初始化种子数据
        var dataSeeder = context.ServiceProvider.GetRequiredService<IDataSeeder>();
        AsyncHelper.RunSync(() => dataSeeder.SeedAsync());
    }

    public override void OnApplicationShutdown(ApplicationShutdownContext context)
    {
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;

namespace PackageName.CompanyName.ProjectName.EntityFrameworkCore;

[DependsOn(
    typeof(ProjectNameTestBaseModule),
    typeof(ProjectNameEntityFrameworkCoreModule)
    )]
public class ProjectNameEntityFrameworkCoreTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddEntityFrameworkInMemoryDatabase();

        var databaseName = Guid.NewGuid().ToString();

        Configure<AbpDbContextOptions>(options =>
        {
            options.Configure(abpDbContextConfigurationContext =>
            {
                abpDbContextConfigurationContext.DbContextOptions.EnableDetailedErrors();
                abpDbContextConfigurationContext.DbContextOptions.EnableSensitiveDataLogging();

                abpDbContextConfigurationContext.DbContextOptions.UseInMemoryDatabase(databaseName);
            });
        });

        Configure<AbpUnitOfWorkDefaultOptions>(options =>
        {
            options.TransactionBehavior = UnitOfWorkTransactionBehavior.Disabled; //EF in-memory database does not support transactions
        });
    }
}

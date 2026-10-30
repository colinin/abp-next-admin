using LINGYUN.Abp.BlobManagement.EntityFrameworkCore;
using LINGYUN.Abp.Tests;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;

namespace LINGYUN.Abp.BlobManagement;

[DependsOn(
    typeof(AbpBlobManagementEntityFrameworkCoreModule),
    typeof(AbpTestsBaseModule))]
public class AbpBlobManagementEntityFrameworkCoreTestsModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddEntityFrameworkInMemoryDatabase();

        var databaseName = Guid.NewGuid().ToString("N");

        Configure<AbpDbContextOptions>(options =>
        {
            options.Configure(dbContextConfigurationContext =>
            {
                dbContextConfigurationContext.DbContextOptions.UseInMemoryDatabase(databaseName);
            });
        });

        Configure<AbpUnitOfWorkDefaultOptions>(options =>
        {
            // EF InMemory 不支持事务
            options.TransactionBehavior = UnitOfWorkTransactionBehavior.Disabled;
        });
    }
}

﻿using LINGYUN.Abp.Data.DbMigrator;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace LY.MicroService.WorkflowManagement.EventBus.Handlers;

public class TenantSynchronizer :
        IDistributedEventHandler<TenantCreatedEto>,
        ITransientDependency
{
    protected IDataSeeder DataSeeder { get; }
    protected ICurrentTenant CurrentTenant { get; }
    protected IDbSchemaMigrator DbSchemaMigrator { get; }
    protected IUnitOfWorkManager UnitOfWorkManager { get; }

    protected ILogger<TenantSynchronizer> Logger { get; }

    public TenantSynchronizer(
        IDataSeeder dataSeeder,
        ICurrentTenant currentTenant,
        IDbSchemaMigrator dbSchemaMigrator,
        IUnitOfWorkManager unitOfWorkManager,
        ILogger<TenantSynchronizer> logger)
    {
        DataSeeder = dataSeeder;
        CurrentTenant = currentTenant;
        DbSchemaMigrator = dbSchemaMigrator;
        UnitOfWorkManager = unitOfWorkManager;

        Logger = logger;
    }

    /// <summary>
    /// 租户创建之后需要预置种子数据
    /// </summary>
    /// <param name="eventData"></param>
    /// <returns></returns>
    public async virtual Task HandleEventAsync(TenantCreatedEto eventData)
    {
        using (var unitOfWork = UnitOfWorkManager.Begin())
        {
            using (CurrentTenant.Change(eventData.Id, eventData.Name))
            {
                Logger.LogInformation("Migrating the new tenant database with WorkflowManagement...");
                // 迁移租户数据
                //await DbSchemaMigrator.MigrateAsync<WorkflowManagementDbContext>(
                //    (connectionString, builder) =>
                //    {
                //        builder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

                //        return new WorkflowManagementDbContext(builder.Options);
                //    });
                Logger.LogInformation("Migrated the new tenant database with WorkflowManagement...");

                await DataSeeder.SeedAsync(new DataSeedContext(eventData.Id));

                await unitOfWork.SaveChangesAsync();
            }
        }
    }
}

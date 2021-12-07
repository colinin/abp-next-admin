using LINGYUN.Abp.Data.DbMigrator;
using LINGYUN.Abp.MultiTenancy;
using LY.MicroService.BackendAdmin.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;
using Volo.Abp.TenantManagement;
using Volo.Abp.Uow;

namespace LY.MicroService.BackendAdmin.EventBus.Handlers;

public class TenantSynchronizer :
        IDistributedEventHandler<CreateEventData>,
        IDistributedEventHandler<EntityDeletedEto<TenantEto>>,
        ITransientDependency
{
    protected ILogger<TenantSynchronizer> Logger { get; }
    protected ICurrentTenant CurrentTenant { get; }
    protected IGuidGenerator GuidGenerator { get; }
    protected IUnitOfWorkManager UnitOfWorkManager { get; }
    protected IDbSchemaMigrator DbSchemaMigrator { get; }
    protected IPermissionGrantRepository PermissionGrantRepository { get; }
    protected IPermissionDefinitionManager PermissionDefinitionManager { get; }
    public TenantSynchronizer(
        ICurrentTenant currentTenant,
        IGuidGenerator guidGenerator,
        IUnitOfWorkManager unitOfWorkManager,
        IDbSchemaMigrator dbSchemaMigrator,
        IPermissionGrantRepository permissionGrantRepository,
        IPermissionDefinitionManager permissionDefinitionManager,
        ILogger<TenantSynchronizer> logger)
    {
        CurrentTenant = currentTenant;
        GuidGenerator = guidGenerator;
        UnitOfWorkManager = unitOfWorkManager;
        DbSchemaMigrator = dbSchemaMigrator;
        PermissionGrantRepository = permissionGrantRepository;
        PermissionDefinitionManager = permissionDefinitionManager;

        Logger = logger;
    }

    public async Task HandleEventAsync(EntityDeletedEto<TenantEto> eventData)
    {
        using var unitOfWork = UnitOfWorkManager.Begin();
        // 订阅租户删除事件,删除管理员角色所有权限
        // TODO: 租户貌似不存在了,删除应该会失败
        // 有缓存存在的话,可以获取到租户连接字符串
        using (CurrentTenant.Change(eventData.Entity.Id))
        {
            // var grantPermissions = await PermissionGrantRepository.GetListAsync("R", "admin");

            // EfCore MySql 批量删除还是一条一条的语句?
            // PermissionGrantRepository.GetDbSet().RemoveRange(grantPermissions);
            var dbContext = await PermissionGrantRepository.GetDbContextAsync();
            var permissionEntityType = dbContext.Model.FindEntityType(typeof(PermissionGrant));
            var permissionTableName = permissionEntityType.GetTableName();
            var batchRmovePermissionSql = string.Empty;
            if (dbContext.Database.IsMySql())
            {
                batchRmovePermissionSql = BuildMySqlBatchDeleteScript(permissionTableName, eventData.Entity.Id);
            }
            else
            {
                batchRmovePermissionSql = BuildSqlServerBatchDeleteScript(permissionTableName, eventData.Entity.Id);
            }

            await dbContext.Database.ExecuteSqlRawAsync(batchRmovePermissionSql);

            await unitOfWork.SaveChangesAsync();
        }
    }

    public async Task HandleEventAsync(CreateEventData eventData)
    {
        using (var unitOfWork = UnitOfWorkManager.Begin())
        {
            // 订阅租户新增事件,置入管理员角色所有权限
            using (CurrentTenant.Change(eventData.Id, eventData.Name))
            {
                Logger.LogInformation("Migrating the new tenant database with platform...");
                // 迁移租户数据
                await DbSchemaMigrator.MigrateAsync<BackendAdminMigrationsDbContext>(
                    (connectionString, builder) =>
                    {
                        builder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

                        return new BackendAdminMigrationsDbContext(builder.Options);
                    });

                Logger.LogInformation("Migrated the new tenant database with platform.");

                Logger.LogInformation("Seeding the new tenant admin role permissions...");
                var definitionPermissions = PermissionDefinitionManager.GetPermissions();
                var grantPermissions = definitionPermissions
                    .Where(p => p.MultiTenancySide.HasFlag(MultiTenancySides.Tenant))
                    .Select(p => p.Name).ToArray();
                //var grantPermissions = new List<PermissionGrant>();
                //foreach (var permission in definitionPermissions)
                //{
                //    var permissionGrant = new PermissionGrant(GuidGenerator.Create(),
                //            permission.Name, "R", "admin", eventData.Id);
                //    grantPermissions.Add(permissionGrant);
                //}
                // TODO: MySql 批量新增还是一条一条的语句?
                // await PermissionGrantRepository.GetDbSet().AddRangeAsync(grantPermissions);

                var dbContext = await PermissionGrantRepository.GetDbContextAsync();
                var permissionEntityType = dbContext.Model.FindEntityType(typeof(PermissionGrant));
                var permissionTableName = permissionEntityType.GetTableName();
                var batchInsertPermissionSql = string.Empty;
                if (dbContext.Database.IsMySql())
                {
                    batchInsertPermissionSql = BuildMySqlBatchInsertScript(permissionTableName, eventData.Id, grantPermissions);
                }
                else
                {
                    batchInsertPermissionSql = BuildSqlServerBatchInsertScript(permissionTableName, eventData.Id, grantPermissions);
                }
                await dbContext.Database.ExecuteSqlRawAsync(batchInsertPermissionSql);

                await unitOfWork.SaveChangesAsync();

                Logger.LogInformation("The new tenant permissions data initialized!");
            }
        }
    }

    protected virtual string BuildMySqlBatchInsertScript(string tableName, Guid tenantId, string[] permissions)
    {
        var batchInsertPermissionSql = new StringBuilder(128);
        batchInsertPermissionSql.AppendLine($"INSERT INTO `{tableName}`(`Id`, `TenantId`, `Name`, `ProviderName`, `ProviderKey`)");
        batchInsertPermissionSql.AppendLine("VALUES");
        for (int i = 0; i < permissions.Length; i++)
        {
            batchInsertPermissionSql.AppendLine($"(UUID(), '{tenantId}','{permissions[i]}','R','admin')");
            if (i < permissions.Length - 1)
            {
                batchInsertPermissionSql.AppendLine(",");
            }
        }
        return batchInsertPermissionSql.ToString();
    }

    protected virtual string BuildSqlServerBatchInsertScript(string tableName, Guid tenantId, string[] permissions)
    {
        var batchInsertPermissionSql = new StringBuilder(128);
        batchInsertPermissionSql.AppendLine($"INSERT INTO {tableName}(Id, TenantId, Name, ProviderName, ProviderKey)");
        batchInsertPermissionSql.Append("VALUES");
        for (int i = 0; i < permissions.Length; i++)
        {
            batchInsertPermissionSql.AppendLine($"(NEWID(), '{tenantId}','{permissions[i]}','R','admin')");
            if (i < permissions.Length - 1)
            {
                batchInsertPermissionSql.AppendLine(",");
            }
        }
        return batchInsertPermissionSql.ToString();
    }

    protected virtual string BuildMySqlBatchDeleteScript(string tableName, Guid tenantId)
    {
        var batchRemovePermissionSql = new StringBuilder(128);
        batchRemovePermissionSql.AppendLine($"DELETE FROM `{tableName}` WHERE `TenantId` = '{tenantId}'");
        batchRemovePermissionSql.AppendLine("AND `ProviderName`='R' AND `ProviderKey`='admin'");
        return batchRemovePermissionSql.ToString();
    }

    protected virtual string BuildSqlServerBatchDeleteScript(string tableName, Guid tenantId)
    {
        var batchRemovePermissionSql = new StringBuilder(128);
        batchRemovePermissionSql.AppendLine($"DELETE {tableName} WHERE TenantId = '{tenantId}'");
        batchRemovePermissionSql.AppendLine("AND ProviderName='R' AND ProviderKey='admin'");
        return batchRemovePermissionSql.ToString();
    }
}

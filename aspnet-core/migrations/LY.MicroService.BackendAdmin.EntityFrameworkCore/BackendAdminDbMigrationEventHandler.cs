using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Migrations;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Uow;

namespace LY.MicroService.BackendAdmin.EntityFrameworkCore;

public class BackendAdminDbMigrationEventHandler : EfCoreDatabaseMigrationEventHandlerBase<BackendAdminMigrationsDbContext>
{
    private const string ModelDatabaseProviderAnnotationKey = "_Abp_DatabaseProvider";

    protected IPermissionGrantRepository PermissionGrantRepository { get; }
    protected IPermissionDefinitionManager PermissionDefinitionManager { get; }

    public BackendAdminDbMigrationEventHandler(
        ICurrentTenant currentTenant, 
        IUnitOfWorkManager unitOfWorkManager, 
        ITenantStore tenantStore, 
        IDistributedEventBus distributedEventBus, 
        ILoggerFactory loggerFactory,
        IPermissionGrantRepository permissionGrantRepository,
        IPermissionDefinitionManager permissionDefinitionManager) 
        : base(
            ConnectionStringNameAttribute.GetConnStringName<BackendAdminMigrationsDbContext>(), 
            currentTenant, unitOfWorkManager, tenantStore, distributedEventBus, loggerFactory)
    {
        PermissionGrantRepository = permissionGrantRepository;
        PermissionDefinitionManager = permissionDefinitionManager;
    }

    protected async override Task AfterTenantConnectionStringUpdated(TenantConnectionStringUpdatedEto eventData, bool schemaMigrated)
    {
        if (!schemaMigrated ||
            string.Equals(eventData.ConnectionStringName, AbpPermissionManagementDbProperties.ConnectionStringName, StringComparison.InvariantCultureIgnoreCase))
        {
            return;
        }

        await SeedAdminRolePermissionsAsync(eventData.Id);
    }

    protected async override Task AfterTenantCreated(TenantCreatedEto eventData, bool schemaMigrated)
    {
        if (!schemaMigrated)
        {
            return;
        }

        await SeedAdminRolePermissionsAsync(eventData.Id);
    }

    protected async virtual Task SeedAdminRolePermissionsAsync(Guid tenantId)
    {
        using (CurrentTenant.Change(tenantId))
        {
            Logger.LogInformation("Seeding the new tenant admin role permissions...");
            var definitionPermissions = await PermissionDefinitionManager.GetPermissionsAsync();
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
            var dbProvider = (EfCoreDatabaseProvider?)dbContext.Model[ModelDatabaseProviderAnnotationKey];
            if (dbProvider != null)
            {
                var permissionEntityType = dbContext.Model.FindEntityType(typeof(PermissionGrant));
                var permissionTableName = permissionEntityType.GetTableName();
                var batchInsertPermissionSql = string.Empty;
                switch (dbProvider)
                {
                    case EfCoreDatabaseProvider.MySql:
                        batchInsertPermissionSql = BuildMySqlBatchInsertScript(permissionTableName, tenantId, grantPermissions);
                        break;
                    case EfCoreDatabaseProvider.SqlServer:
                        batchInsertPermissionSql = BuildSqlServerBatchInsertScript(permissionTableName, tenantId, grantPermissions);
                        break;
                    default:
                        Logger.LogWarning($"Tenant permissions data has not initialized, Because database provider: {dbProvider} batch statements are not defined!");
                        return;
                }
                await dbContext.Database.ExecuteSqlRawAsync(batchInsertPermissionSql);

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

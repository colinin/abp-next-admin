using LINGYUN.Common.EventBus.Tenants;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Uow;

namespace LINGYUN.Platform.EventBus.Handlers
{
    public class TenantCreateEventHandler : IDistributedEventHandler<CreateEventData>, ITransientDependency
    {
        protected ICurrentTenant CurrentTenant { get; }
        protected IGuidGenerator GuidGenerator { get; }
        protected IUnitOfWorkManager UnitOfWorkManager { get; }
        protected IPermissionGrantRepository PermissionGrantRepository { get; }
        protected IPermissionDefinitionManager PermissionDefinitionManager { get; }
        public TenantCreateEventHandler(
            ICurrentTenant currentTenant,
            IGuidGenerator guidGenerator,
            IUnitOfWorkManager unitOfWorkManager,
            IPermissionGrantRepository permissionGrantRepository,
            IPermissionDefinitionManager permissionDefinitionManager)
        {
            CurrentTenant = currentTenant;
            GuidGenerator = guidGenerator;
            UnitOfWorkManager = unitOfWorkManager;
            PermissionGrantRepository = permissionGrantRepository;
            PermissionDefinitionManager = permissionDefinitionManager;
        }

        public async Task HandleEventAsync(CreateEventData eventData)
        {
            using (var unitOfWork = UnitOfWorkManager.Begin())
            {
                // 订阅租户新增事件,置入管理员角色所有权限
                using (CurrentTenant.Change(eventData.Id, eventData.Name))
                {
                    var definitionPermissions = PermissionDefinitionManager.GetPermissions();
                    var grantPermissions = definitionPermissions.Select(p => p.Name).ToArray();
                    //var grantPermissions = new List<PermissionGrant>();
                    //foreach (var permission in definitionPermissions)
                    //{
                    //    var permissionGrant = new PermissionGrant(GuidGenerator.Create(),
                    //            permission.Name, "R", "admin", eventData.Id);
                    //    grantPermissions.Add(permissionGrant);
                    //}
                    // TODO: MySql 批量新增还是一条一条的语句?
                    // await PermissionGrantRepository.GetDbSet().AddRangeAsync(grantPermissions);

                    var permissionEntityType = PermissionGrantRepository.GetDbContext()
                        .Model.FindEntityType(typeof(PermissionGrant));
                    var permissionTableName = permissionEntityType.GetTableName();
                    var batchInsertPermissionSql = string.Empty;
                    if (PermissionGrantRepository.GetDbContext().Database.IsMySql())
                    {
                        batchInsertPermissionSql = BuildMySqlBatchInsertScript(permissionTableName, eventData.Id, grantPermissions);
                    }
                    else
                    {
                        batchInsertPermissionSql = BuildSqlServerBatchInsertScript(permissionTableName, eventData.Id, grantPermissions);
                    }
                    await PermissionGrantRepository.GetDbContext().Database.ExecuteSqlRawAsync(batchInsertPermissionSql);

                    await unitOfWork.SaveChangesAsync();
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
                if(i < permissions.Length - 1)
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
    }
}

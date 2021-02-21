using Microsoft.EntityFrameworkCore;
using System;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;
using Volo.Abp.TenantManagement;
using Volo.Abp.Uow;

namespace LINGYUN.Abp.BackendAdmin.EventBus.Handlers
{
    public class TenantDeleteEventHandler : IDistributedEventHandler<EntityDeletedEto<TenantEto>>, ITransientDependency
    {
        protected ICurrentTenant CurrentTenant { get; }
        protected IGuidGenerator GuidGenerator { get; }
        protected IUnitOfWorkManager UnitOfWorkManager { get; }
        protected IPermissionGrantRepository PermissionGrantRepository { get; }
        public TenantDeleteEventHandler(
            ICurrentTenant currentTenant,
            IGuidGenerator guidGenerator,
            IUnitOfWorkManager unitOfWorkManager,
            IPermissionGrantRepository permissionGrantRepository)
        {
            CurrentTenant = currentTenant;
            GuidGenerator = guidGenerator;
            UnitOfWorkManager = unitOfWorkManager;
            PermissionGrantRepository = permissionGrantRepository;
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
}

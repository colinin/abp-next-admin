using LINGYUN.Common.EventBus.Tenants;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Uow;

namespace LINGYUN.Platform.EventBus.Handlers
{
    public class TenantDeleteEventHandler : IDistributedEventHandler<DeleteEventData>, ITransientDependency
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

        public async Task HandleEventAsync(DeleteEventData eventData)
        {
            using var unitOfWork = UnitOfWorkManager.Begin();
            // 订阅租户删除事件,删除管理员角色所有权限
            // TODO: 租户貌似不存在了,删除应该会失败
            using (CurrentTenant.Change(eventData.Id))
            {
                // var grantPermissions = await PermissionGrantRepository.GetListAsync("R", "admin");

                // EfCore MySql 批量删除还是一条一条的语句?
                // PermissionGrantRepository.GetDbSet().RemoveRange(grantPermissions);
                var permissionEntityType = PermissionGrantRepository.GetDbContext().Model.FindEntityType(typeof(PermissionGrant));
                var permissionTableName = permissionEntityType.GetTableName();
                var batchRmovePermissionSql = string.Empty;
                if (PermissionGrantRepository.GetDbContext().Database.IsMySql())
                {
                    batchRmovePermissionSql = BuildMySqlBatchDeleteScript(permissionTableName, eventData.Id);
                }
                else
                {
                    batchRmovePermissionSql = BuildSqlServerBatchDeleteScript(permissionTableName, eventData.Id);
                }

                await PermissionGrantRepository.GetDbContext().Database
                    .ExecuteSqlRawAsync(batchRmovePermissionSql);

                await unitOfWork.CompleteAsync();
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

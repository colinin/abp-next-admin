using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Auditing;

namespace LINGYUN.Abp.AuditLogging
{
    public interface IEntityChangeStore
    {
        Task<EntityChange> GetAsync(
            Guid entityChangeId, 
            CancellationToken cancellationToken = default);

        Task<long> GetCountAsync(
            Guid? auditLogId = null,
            DateTime? startTime = null,
            DateTime? endTime = null, 
            EntityChangeType? changeType = null, 
            string entityId = null, 
            string entityTypeFullName = null, 
            CancellationToken cancellationToken = default);

        Task<List<EntityChange>> GetListAsync(
            string sorting = null, 
            int maxResultCount = 50, 
            int skipCount = 0, 
            Guid? auditLogId = null, 
            DateTime? startTime = null, 
            DateTime? endTime = null, 
            EntityChangeType? changeType = null, 
            string entityId = null, 
            string entityTypeFullName = null, 
            bool includeDetails = false,
            CancellationToken cancellationToken = default);

        Task<EntityChangeWithUsername> GetWithUsernameAsync(
            Guid entityChangeId,
            CancellationToken cancellationToken = default);

        Task<List<EntityChangeWithUsername>> GetWithUsernameAsync(
            string entityId, 
            string entityTypeFullName, 
            CancellationToken cancellationToken = default);
    }
}

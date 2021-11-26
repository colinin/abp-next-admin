using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.AuditLogging
{
    [Dependency(TryRegister = true)]
    public class DefaultEntityChangeStore : IEntityChangeStore, ISingletonDependency
    {
        public Task<EntityChange> GetAsync(Guid entityChangeId, CancellationToken cancellationToken = default)
        {
            EntityChange entityChange = null;
            return Task.FromResult(entityChange);
        }

        public Task<long> GetCountAsync(Guid? auditLogId = null, DateTime? startTime = null, DateTime? endTime = null, EntityChangeType? changeType = null, string entityId = null, string entityTypeFullName = null, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(0L);
        }

        public Task<List<EntityChange>> GetListAsync(string sorting = null, int maxResultCount = 50, int skipCount = 0, Guid? auditLogId = null, DateTime? startTime = null, DateTime? endTime = null, EntityChangeType? changeType = null, string entityId = null, string entityTypeFullName = null, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new List<EntityChange>());
        }

        public Task<EntityChangeWithUsername> GetWithUsernameAsync(Guid entityChangeId, CancellationToken cancellationToken = default)
        {
            EntityChangeWithUsername entityChange = null;
            return Task.FromResult(entityChange);
        }

        public Task<List<EntityChangeWithUsername>> GetWithUsernameAsync(string entityId, string entityTypeFullName, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new List<EntityChangeWithUsername>());
        }
    }
}

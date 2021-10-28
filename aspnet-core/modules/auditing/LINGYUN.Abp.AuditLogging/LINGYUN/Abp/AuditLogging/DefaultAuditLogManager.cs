using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.AuditLogging
{
    [Dependency(TryRegister = true)]
    public class DefaultAuditLogManager : IAuditLogManager, ISingletonDependency
    {
        public ILogger<DefaultAuditLogManager> Logger { protected get; set; }

        public DefaultAuditLogManager()
        {
            Logger = NullLogger<DefaultAuditLogManager>.Instance;
        }

        public virtual Task<long> GetCountAsync(
            DateTime? startTime = null,
            DateTime? endTime = null,
            string httpMethod = null,
            string url = null,
            Guid? userId = null,
            string userName = null,
            string applicationName = null,
            string correlationId = null,
            string clientId = null,
            string clientIpAddress = null,
            int? maxExecutionDuration = null,
            int? minExecutionDuration = null,
            bool? hasException = null,
            HttpStatusCode? httpStatusCode = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Logger.LogDebug("No audit log manager is available!");
            return Task.FromResult(0L);
        }

        public virtual Task<List<AuditLog>> GetListAsync(
            string sorting = null,
            int maxResultCount = 50,
            int skipCount = 0,
            DateTime? startTime = null,
            DateTime? endTime = null,
            string httpMethod = null,
            string url = null,
            Guid? userId = null,
            string userName = null,
            string applicationName = null,
            string correlationId = null,
            string clientId = null,
            string clientIpAddress = null,
            int? maxExecutionDuration = null,
            int? minExecutionDuration = null,
            bool? hasException = null,
            HttpStatusCode? httpStatusCode = null,
            bool includeDetails = false,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Logger.LogDebug("No audit log manager is available!");
            return Task.FromResult(new List<AuditLog>());
        }

        public virtual Task<string> SaveAsync(
            AuditLogInfo auditInfo, 
            CancellationToken cancellationToken = default)
        {
            Logger.LogDebug("No audit log manager is available and is written to the local log by default");
            Logger.LogInformation(auditInfo.ToString());

            return Task.FromResult("");
        }

        public virtual Task<AuditLog> GetAsync(
            Guid id, 
            bool includeDetails = false, 
            CancellationToken cancellationToken = default)
        {
            Logger.LogDebug("No audit log manager is available!");

            AuditLog auditLog = null;
            return Task.FromResult(auditLog);
        }

        public virtual Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            Logger.LogDebug("No audit log manager is available!");
            return Task.CompletedTask;
        }
    }
}

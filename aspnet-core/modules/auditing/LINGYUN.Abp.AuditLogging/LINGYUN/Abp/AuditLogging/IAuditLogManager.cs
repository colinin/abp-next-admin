using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Auditing;

namespace LINGYUN.Abp.AuditLogging
{
    public interface IAuditLogManager
    {
        Task<AuditLog> GetAsync(
            Guid id,
            bool includeDetails = false,
            CancellationToken cancellationToken = default(CancellationToken));

        Task DeleteAsync(
            Guid id,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<string> SaveAsync(
            AuditLogInfo auditInfo,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<long> GetCountAsync(
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
            CancellationToken cancellationToken = default(CancellationToken));

        Task<List<AuditLog>> GetListAsync(
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
            CancellationToken cancellationToken = default(CancellationToken));

    }
}

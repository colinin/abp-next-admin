using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.SecurityLog;

namespace LINGYUN.Abp.AuditLogging
{
    public interface ISecurityLogManager
    {
        Task<SecurityLog> GetAsync(
            Guid id,
            bool includeDetails = false,
            CancellationToken cancellationToken = default(CancellationToken));

        Task DeleteAsync(
            Guid id,
            CancellationToken cancellationToken = default(CancellationToken));

        Task SaveAsync(
            SecurityLogInfo securityLogInfo,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<List<SecurityLog>> GetListAsync(
            string sorting = null, 
            int maxResultCount = 50, 
            int skipCount = 0, 
            DateTime? startTime = null, 
            DateTime? endTime = null, 
            string applicationName = null,
            string identity = null, 
            string action = null, 
            Guid? userId = null, 
            string userName = null,
            string clientId = null,
            string clientIpAddress = null,
            string correlationId = null, 
            bool includeDetails = false,
            CancellationToken cancellationToken = default(CancellationToken));


        Task<long> GetCountAsync(
            DateTime? startTime = null,
            DateTime? endTime = null,
            string applicationName = null, 
            string identity = null, 
            string action = null, 
            Guid? userId = null, 
            string userName = null, 
            string clientId = null,
            string clientIpAddress = null,
            string correlationId = null,
            CancellationToken cancellationToken = default(CancellationToken));

    }
}

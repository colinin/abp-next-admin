using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.SecurityLog;

namespace LINGYUN.Abp.AuditLogging
{
    [Dependency(TryRegister = true)]
    public class DefaultSecurityLogManager : ISecurityLogManager, ISingletonDependency
    {
        public ILogger<DefaultSecurityLogManager> Logger { protected get; set; }

        public DefaultSecurityLogManager()
        {
            Logger = NullLogger<DefaultSecurityLogManager>.Instance;
        }

        public Task<long> GetCountAsync(
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
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Logger.LogDebug("No security log manager is available!");
            return Task.FromResult(0L);
        }

        public Task<List<SecurityLog>> GetListAsync(
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
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Logger.LogDebug("No security log manager is available!");
            return Task.FromResult(new List<SecurityLog>());
        }

        public Task SaveAsync(
            SecurityLogInfo securityLogInfo,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Logger.LogDebug("No security log manager is available and is written to the local log by default");
            Logger.LogInformation(securityLogInfo.ToString());

            return Task.CompletedTask;
        }

        public virtual Task<SecurityLog> GetAsync(
            Guid id, 
            bool includeDetails = false, 
            CancellationToken cancellationToken = default)
        {
            Logger.LogDebug("No security log manager is available!");

            SecurityLog securityLog = null;
            return Task.FromResult(securityLog);
        }

        public virtual Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            Logger.LogDebug("No security log manager is available!");
            return Task.CompletedTask;
        }
    }
}

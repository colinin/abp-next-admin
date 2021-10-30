using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Logging
{
    [Dependency(TryRegister = true)]
    public class DefaultLoggingManager : ILoggingManager, ISingletonDependency
    {
        public ILogger<DefaultLoggingManager> Logger { protected get; set; }

        public DefaultLoggingManager()
        {
            Logger = NullLogger<DefaultLoggingManager>.Instance;
        }

        public Task<LogInfo> GetAsync(string id, CancellationToken cancellationToken = default)
        {
            Logger.LogDebug("No logging manager is available!");
            LogInfo logInfo = null;
            return Task.FromResult(logInfo);
        }

        public Task<long> GetCountAsync(
            DateTime? startTime = null, 
            DateTime? endTime = null,
            LogLevel? level = null,
            string machineName = null,
            string environment = null,
            string application = null,
            string context = null, 
            string requestId = null,
            string requestPath = null, 
            string correlationId = null, 
            int? processId = null, 
            int? threadId = null,
            bool? hasException = null,
            CancellationToken cancellationToken = default)
        {
            Logger.LogDebug("No logging manager is available!");
            return Task.FromResult(0L);
        }

        public Task<List<LogInfo>> GetListAsync(
            string sorting = null,
            int maxResultCount = 50, 
            int skipCount = 0, 
            DateTime? startTime = null,
            DateTime? endTime = null,
            LogLevel? level = null,
             string machineName = null,
            string environment = null,
            string application = null,
            string context = null,
            string requestId = null, 
            string requestPath = null,
            string correlationId = null, 
            int? processId = null, 
            int? threadId = null,
            bool? hasException = null,
            bool includeDetails = false, 
            CancellationToken cancellationToken = default)
        {
            Logger.LogDebug("No logging manager is available!");
            return Task.FromResult(new List<LogInfo>());
        }
    }
}

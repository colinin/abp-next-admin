using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Logging
{
    public interface ILoggingManager
    {
        Task<LogInfo> GetAsync(
            string id,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<long> GetCountAsync(
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
            CancellationToken cancellationToken = default(CancellationToken));

        Task<List<LogInfo>> GetListAsync(
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
            CancellationToken cancellationToken = default(CancellationToken));
    }
}

using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.SecurityLog;

namespace LINGYUN.Abp.AuditLogging;

[Dependency(TryRegister = true)]
public class LoggerSecurityLogWriter : ISecurityLogWriter, ISingletonDependency
{
    private readonly ILogger<LoggerSecurityLogWriter> _logger;
    public LoggerSecurityLogWriter(ILogger<LoggerSecurityLogWriter> logger)
    {
        _logger = logger;
    }

    public async virtual Task BulkWriteAsync(IEnumerable<SecurityLogInfo> securityLogInfos, CancellationToken cancellationToken = default)
    {
        foreach (var securityLogInfo in securityLogInfos)
        {
            await WriteAsync(securityLogInfo, cancellationToken);
        }
    }

    public virtual Task WriteAsync(SecurityLogInfo securityLogInfo, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(securityLogInfo.ToString());

        return Task.CompletedTask;
    }
}

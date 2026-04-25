using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.SecurityLog;

namespace LINGYUN.Abp.AuditLogging;

[Dependency(ReplaceServices = true)]
public class SecurityLogStore : ISecurityLogStore, ITransientDependency
{
    private readonly AbpSecurityLogOptions _securityLogOptions;
    private readonly AbpAuditLoggingOptions _loggingOptions;
    private readonly ISecurityLogWriter _securityLogWriter;
    private readonly ISecurityLogQueue _securityLogQueue;
    private readonly ILogger<SecurityLogStore> _logger;

    public SecurityLogStore(
        IOptionsMonitor<AbpSecurityLogOptions> securityLogOptions,
        IOptionsMonitor<AbpAuditLoggingOptions> loggingOptions,
        ISecurityLogWriter securityLogWriter,
        ISecurityLogQueue securityLogQueue,
        ILogger<SecurityLogStore> logger)
    {
        _securityLogOptions = securityLogOptions.CurrentValue;
        _loggingOptions = loggingOptions.CurrentValue;
        _securityLogWriter = securityLogWriter;
        _securityLogQueue = securityLogQueue;
        _logger = logger;
    }

    public async virtual Task SaveAsync(SecurityLogInfo securityLogInfo)
    {
        if (!_loggingOptions.IsAuditLogEnabled || !_securityLogOptions.IsEnabled)
        {
            _logger.LogInformation(securityLogInfo.ToString());
            return;
        }
        if (_loggingOptions.UseSecurityLogQueue)
        {
            await _securityLogQueue.EnqueueAsync(securityLogInfo);
        }
        else
        {
            await _securityLogWriter.WriteAsync(securityLogInfo);
        }
    }
}

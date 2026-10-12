using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.SecurityLog;

namespace LINGYUN.Abp.AuditLogging;

[Dependency(ReplaceServices = true)]
public class SecurityLogStore : ISecurityLogStore, ITransientDependency
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly AbpSecurityLogOptions _securityLogOptions;
    private readonly AbpAuditLoggingOptions _loggingOptions;
    private readonly ISecurityLogWriter _securityLogWriter;
    private readonly ISecurityLogQueue _securityLogQueue;
    private readonly ILogger<SecurityLogStore> _logger;

    public SecurityLogStore(
        IServiceScopeFactory serviceScopeFactory,
        IOptionsMonitor<AbpSecurityLogOptions> securityLogOptions,
        IOptionsMonitor<AbpAuditLoggingOptions> loggingOptions,
        ISecurityLogWriter securityLogWriter,
        ISecurityLogQueue securityLogQueue,
        ILogger<SecurityLogStore> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
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

        if (_loggingOptions.SecurityLogInfoContributors.Any())
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var context = new SecurityLogInfoContributorContext(scope.ServiceProvider, securityLogInfo);
                foreach (var contributor in _loggingOptions.SecurityLogInfoContributors)
                {
                    await contributor.ContributeAsync(context);
                }
            }
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

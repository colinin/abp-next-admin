using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.AuditLogging;

[Dependency(ReplaceServices = true)]
public class AuditingStore : IAuditingStore, ITransientDependency
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly AbpAuditLoggingOptions _loggingOptions;
    private readonly IAuditLogWriter _auditLogWriter;
    private readonly IAuditLogQueue _auditLogQueue;
    private readonly ILogger<AuditingStore> _logger;

    public AuditingStore(
        IServiceScopeFactory serviceScopeFactory,
        IOptionsMonitor<AbpAuditLoggingOptions> loggingOptions,
        IAuditLogWriter auditLogWriter,
        IAuditLogQueue auditLogQueue,
        ILogger<AuditingStore> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _loggingOptions = loggingOptions.CurrentValue;
        _auditLogWriter = auditLogWriter;
        _auditLogQueue = auditLogQueue;
        _logger = logger;
    }

    public async virtual Task SaveAsync(AuditLogInfo auditInfo)
    {
        if (!_loggingOptions.IsAuditLogEnabled)
        {
            _logger.LogInformation(auditInfo.ToString());
            return;
        }

        if (_loggingOptions.AuditLogInfoContributors.Any())
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var context = new AuditLogInfoContributorContext(scope.ServiceProvider, auditInfo);
                foreach (var contributor in _loggingOptions.AuditLogInfoContributors)
                {
                    await contributor.ContributeAsync(context);
                }
            }
        }

        if (_loggingOptions.UseAuditLogQueue)
        {
            await _auditLogQueue.EnqueueAsync(auditInfo);
        }
        else
        {
            await _auditLogWriter.WriteAsync(auditInfo);
        }
    }
}

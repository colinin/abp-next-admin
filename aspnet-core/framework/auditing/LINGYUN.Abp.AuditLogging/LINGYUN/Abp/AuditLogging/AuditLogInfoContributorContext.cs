using System;
using Volo.Abp.Auditing;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.AuditLogging;

public class AuditLogInfoContributorContext : IServiceProviderAccessor
{
    public IServiceProvider ServiceProvider { get; }

    public AuditLogInfo AuditLogInfo { get; }

    public AuditLogInfoContributorContext(IServiceProvider serviceProvider, AuditLogInfo auditLogInfo)
    {
        ServiceProvider = serviceProvider;
        AuditLogInfo = auditLogInfo;
    }
}

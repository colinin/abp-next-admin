using System;
using Volo.Abp.DependencyInjection;
using Volo.Abp.SecurityLog;

namespace LINGYUN.Abp.AuditLogging;

public class SecurityLogInfoContributorContext : IServiceProviderAccessor
{
    public IServiceProvider ServiceProvider { get; }

    public SecurityLogInfo SecurityLogInfo { get; }

    public SecurityLogInfoContributorContext(IServiceProvider serviceProvider, SecurityLogInfo securityLogInfo)
    {
        ServiceProvider = serviceProvider;
        SecurityLogInfo = securityLogInfo;
    }
}

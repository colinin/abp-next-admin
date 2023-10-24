using System.Collections.Generic;
using System.Threading;
using Volo.Abp.Auditing;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Guids;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.AuditLogging
{
    [DependsOn(
        typeof(AbpAuditingModule),
        typeof(AbpGuidsModule),
        typeof(AbpExceptionHandlingModule))]
    public class AbpAuditLoggingModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAuditingOptions>(options =>
            {
                options.IgnoredTypes.AddIfNotContains(typeof(CancellationToken));
                options.IgnoredTypes.AddIfNotContains(typeof(CancellationTokenSource));
            });
        }
    }
}

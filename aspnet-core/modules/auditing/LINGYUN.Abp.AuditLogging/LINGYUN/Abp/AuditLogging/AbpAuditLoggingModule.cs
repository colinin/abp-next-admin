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
    }
}

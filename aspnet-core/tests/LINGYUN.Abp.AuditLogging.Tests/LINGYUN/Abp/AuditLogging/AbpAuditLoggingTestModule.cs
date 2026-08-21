using LINGYUN.Abp.Tests;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.AuditLogging;

[DependsOn(
    typeof(AbpTestsBaseModule),
    typeof(AbpAuditLoggingModule))]
public class AbpAuditLoggingTestModule : AbpModule
{
}

using LINGYUN.Abp.AuditLogging;
using LINGYUN.Abp.Logging;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Mapperly;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Auditing;

[DependsOn(
    typeof(AbpMapperlyModule),
    typeof(AbpLoggingModule),
    typeof(AbpAuditLoggingModule),
    typeof(AbpAuditingApplicationContractsModule))]
public class AbpAuditingApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMapperlyObjectMapper<AbpAuditingApplicationModule>();
    }
}

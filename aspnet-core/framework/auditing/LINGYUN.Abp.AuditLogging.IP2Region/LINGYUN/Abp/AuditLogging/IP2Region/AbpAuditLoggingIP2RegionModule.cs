using LINGYUN.Abp.IP2Region;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Auditing;
using Volo.Abp.Modularity;
using Volo.Abp.SecurityLog;

namespace LINGYUN.Abp.AuditLogging.IP2Region;

[DependsOn(
    typeof(AbpIP2RegionModule),
    typeof(AbpAuditLoggingModule))]
public class AbpAuditLoggingIP2RegionModule : AbpModule
{
    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        Configure<AbpAuditLoggingIP2RegionOptions>(configuration.GetSection("AuditLogging:IP2Region"));

        context.Services.Replace(
            ServiceDescriptor.Transient<IAuditingStore, IP2RegionAuditingStore>());
        context.Services.Replace(
            ServiceDescriptor.Transient<ISecurityLogStore, IP2RegionSecurityLogStore>());
    }
}

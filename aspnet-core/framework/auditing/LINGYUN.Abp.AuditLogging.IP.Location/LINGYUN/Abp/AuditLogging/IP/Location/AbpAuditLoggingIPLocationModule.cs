using LINGYUN.Abp.IP.Location;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Auditing;
using Volo.Abp.Modularity;
using Volo.Abp.SecurityLog;

namespace LINGYUN.Abp.AuditLogging.IP.Location;

[DependsOn(
    typeof(AbpIPLocationModule),
    typeof(AbpAuditLoggingModule))]
public class AbpAuditLoggingIPLocationModule : AbpModule
{
    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        Configure<AbpAuditLoggingIPLocationOptions>(configuration.GetSection("AuditLogging:IPLocation"));

        context.Services.Replace(
            ServiceDescriptor.Transient<IAuditingStore, IPLocationAuditingStore>());
        context.Services.Replace(
            ServiceDescriptor.Transient<ISecurityLogStore, IPLocationSecurityLogStore>());
    }
}

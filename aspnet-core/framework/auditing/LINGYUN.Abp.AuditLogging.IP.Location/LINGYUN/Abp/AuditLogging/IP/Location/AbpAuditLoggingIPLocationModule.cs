using LINGYUN.Abp.IP.Location;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

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

        Configure<AbpAuditLoggingOptions>(options =>
        {
            options.AuditLogInfoContributors.Add(new IPLocationAuditLogInfoContributor());

            options.SecurityLogInfoContributors.Add(new IPLocationSecurityLogInfoContributor());
        });
    }
}

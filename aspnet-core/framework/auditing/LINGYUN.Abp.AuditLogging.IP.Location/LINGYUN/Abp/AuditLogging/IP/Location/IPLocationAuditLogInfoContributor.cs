using LINGYUN.Abp.IP.Location;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Volo.Abp.Data;

namespace LINGYUN.Abp.AuditLogging.IP.Location;

public class IPLocationAuditLogInfoContributor : IAuditLogInfoContributor
{
    public async virtual Task ContributeAsync(AuditLogInfoContributorContext context)
    {
        if (context.AuditLogInfo.ClientIpAddress.IsNullOrWhiteSpace())
        {
            return;
        }
        var options = context.ServiceProvider.GetRequiredService<IOptions<AbpAuditLoggingIPLocationOptions>>();
        if (!options.Value.IsEnabled)
        {
            return;
        }

        var ipLocationResolver = context.ServiceProvider.GetRequiredService<IIPLocationResolver>();
        var result = await ipLocationResolver.ResolveAsync(context.AuditLogInfo.ClientIpAddress);

        if (result.Location?.Remarks?.IsNullOrWhiteSpace() == false)
        {
            context.AuditLogInfo.SetProperty(IPLocationConstants.IPLocationKeywords, $"{result.Location.Remarks}");
        }
    }
}

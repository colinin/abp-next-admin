using LINGYUN.Abp.IP.Location;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace LINGYUN.Abp.AuditLogging.IP.Location;

public class IPLocationSecurityLogInfoContributor : ISecurityLogInfoContributor
{
    public async virtual Task ContributeAsync(SecurityLogInfoContributorContext context)
    {
        if (context.SecurityLogInfo.ClientIpAddress.IsNullOrWhiteSpace())
        {
            return;
        }
        var options = context.ServiceProvider.GetRequiredService<IOptions<AbpAuditLoggingIPLocationOptions>>();
        if (!options.Value.IsEnabled)
        {
            return;
        }

        var ipLocationResolver = context.ServiceProvider.GetRequiredService<IIPLocationResolver>();
        var result = await ipLocationResolver.ResolveAsync(context.SecurityLogInfo.ClientIpAddress);

        if (result.Location?.Remarks?.IsNullOrWhiteSpace() == false)
        {
            context.SecurityLogInfo.ExtraProperties.Add(IPLocationConstants.IPLocationKeywords, $"{result.Location.Remarks}");
        }
    }
}

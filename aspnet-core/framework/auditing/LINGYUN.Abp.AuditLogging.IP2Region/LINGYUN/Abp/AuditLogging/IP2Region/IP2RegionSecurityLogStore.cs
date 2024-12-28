using LINGYUN.Abp.IP2Region;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Volo.Abp.SecurityLog;

namespace LINGYUN.Abp.AuditLogging.IP2Region;

public class IP2RegionSecurityLogStore : SecurityLogStore
{
    private readonly AbpAuditLoggingIP2RegionOptions _options;
    private readonly IIpLocationInfoProvider _ipLocationInfoProvider;
    public IP2RegionSecurityLogStore(
        IOptionsMonitor<AbpAuditLoggingIP2RegionOptions> options,
        IIpLocationInfoProvider ipLocationInfoProvider,
        ISecurityLogManager manager) 
        : base(manager)
    {
        _options = options.CurrentValue;
        _ipLocationInfoProvider = ipLocationInfoProvider;
    }

    public async override Task SaveAsync(SecurityLogInfo securityLogInfo)
    {
        if (_options.IsEnabled && !securityLogInfo.ClientIpAddress.IsNullOrWhiteSpace())
        {
            var locationInfo = await _ipLocationInfoProvider.GetLocationInfoAsync(securityLogInfo.ClientIpAddress);
            if (locationInfo?.Remarks?.IsNullOrWhiteSpace() == false)
            {
                securityLogInfo.ExtraProperties.Add("Location", $"{locationInfo.Remarks}");
            }
        }
 
        await base.SaveAsync(securityLogInfo);
    }
}

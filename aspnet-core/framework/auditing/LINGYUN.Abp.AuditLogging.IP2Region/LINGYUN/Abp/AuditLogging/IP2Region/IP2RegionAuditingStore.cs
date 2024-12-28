using LINGYUN.Abp.IP2Region;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Volo.Abp.Auditing;

namespace LINGYUN.Abp.AuditLogging.IP2Region;
public class IP2RegionAuditingStore : AuditingStore
{
    private readonly AbpAuditLoggingIP2RegionOptions _options;
    private readonly IIpLocationInfoProvider _ipLocationInfoProvider;
    public IP2RegionAuditingStore(
        IOptionsMonitor<AbpAuditLoggingIP2RegionOptions> options,
        IIpLocationInfoProvider ipLocationInfoProvider,
        IAuditLogManager manager)
        : base(manager)
    {
        _options = options.CurrentValue;
        _ipLocationInfoProvider = ipLocationInfoProvider;
    }

    public async override Task SaveAsync(AuditLogInfo auditInfo)
    {
        if (_options.IsEnabled && !auditInfo.ClientIpAddress.IsNullOrWhiteSpace())
        {
            var locationInfo = await _ipLocationInfoProvider.GetLocationInfoAsync(auditInfo.ClientIpAddress);
            if (locationInfo?.Remarks?.IsNullOrWhiteSpace() == false)
            {
                auditInfo.ExtraProperties.Add("Location", $"{locationInfo.Remarks}");
            }
        }
        await base.SaveAsync(auditInfo);
    }
}

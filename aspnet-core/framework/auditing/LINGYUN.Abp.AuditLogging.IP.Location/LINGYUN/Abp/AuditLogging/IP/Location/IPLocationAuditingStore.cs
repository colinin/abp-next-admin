using LINGYUN.Abp.IP.Location;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Volo.Abp.Auditing;

namespace LINGYUN.Abp.AuditLogging.IP.Location;
public class IPLocationAuditingStore : AuditingStore
{
    private readonly AbpAuditLoggingIPLocationOptions _options;
    private readonly IIPLocationResolver _iPLocationResolver;
    public IPLocationAuditingStore(
        IOptionsMonitor<AbpAuditLoggingIPLocationOptions> options,
        IIPLocationResolver iPLocationResolver,
        IAuditLogManager manager)
        : base(manager)
    {
        _options = options.CurrentValue;
        _iPLocationResolver = iPLocationResolver;
    }

    public async override Task SaveAsync(AuditLogInfo auditInfo)
    {
        if (_options.IsEnabled && !auditInfo.ClientIpAddress.IsNullOrWhiteSpace())
        {
            var result = await _iPLocationResolver.ResolveAsync(auditInfo.ClientIpAddress);

            if (result.Location?.Remarks?.IsNullOrWhiteSpace() == false)
            {
                auditInfo.ExtraProperties.Add("Location", $"{result.Location.Remarks}");
            }
        }
        await base.SaveAsync(auditInfo);
    }
}

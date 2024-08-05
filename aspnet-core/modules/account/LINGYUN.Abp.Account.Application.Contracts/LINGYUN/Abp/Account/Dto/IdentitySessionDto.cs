using System;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.Account;
public class IdentitySessionDto : EntityDto<Guid>
{
    public string SessionId { get; set; }

    public string Device { get; set; }

    public string DeviceInfo { get; set; }

    public string ClientId { get; set; }

    public string IpAddresses { get; set; }

    public DateTime SignedIn { get; set; }

    public DateTime? LastAccessed { get; set; }
}

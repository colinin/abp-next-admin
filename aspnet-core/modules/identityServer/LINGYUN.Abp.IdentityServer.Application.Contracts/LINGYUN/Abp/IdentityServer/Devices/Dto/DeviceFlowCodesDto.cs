using System;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.IdentityServer.Devices;

public class DeviceFlowCodesDto : ExtensibleCreationAuditedEntityDto<Guid>
{
    public string DeviceCode { get; set; } = default!;

    public string UserCode { get; set; } = default!;

    public string? SubjectId { get; set; }

    public string? SessionId { get; set; }

    public string ClientId { get; set; } = default!;

    public string? Description { get; set; }

    public DateTime? Expiration { get; set; }

    public string Data { get; set; } = default!;
}

using System;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.IdentityServer.Grants;

public class PersistedGrantDto : ExtensibleEntityDto<Guid>
{
    public string Key { get; set; } = default!;

    public string Type { get; set; } = default!;

    public string? SubjectId { get; set; }

    public string? SessionId { get; set; }

    public string? Description { get; set; }

    public DateTime? ConsumedTime { get; set; }

    public string ClientId { get; set; } = default!;

    public DateTime CreationTime { get; set; }

    public DateTime? Expiration { get; set; }

    public string Data { get; set; } = default!;
}

using System;

namespace LINGYUN.Abp.IdentityServer;

public class SecretDto
{
    public string Type { get; set; } = default!;

    public string Value { get; set; } = default!;

    public string? Description { get; set; }

    public DateTime? Expiration { get; set; }
}

namespace LINGYUN.Abp.IdentityServer.Clients;

public class ClientClaimDto
{
    public string Type { get; set; } = default!;

    public string? Value { get; set; }
}

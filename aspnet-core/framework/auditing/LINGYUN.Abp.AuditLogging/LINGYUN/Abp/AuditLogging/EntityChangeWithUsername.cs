namespace LINGYUN.Abp.AuditLogging;

public class EntityChangeWithUsername
{
    public EntityChange EntityChange { get; set; } = default!;

    public string? UserName { get; set; }
}

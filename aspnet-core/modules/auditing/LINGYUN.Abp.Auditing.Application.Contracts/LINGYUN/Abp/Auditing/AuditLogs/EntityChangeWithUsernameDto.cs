namespace LINGYUN.Abp.Auditing.AuditLogs;
public class EntityChangeWithUsernameDto
{
    public EntityChangeDto EntityChange { get; set; } = default!;

    public string UserName { get; set; } = default!;
}

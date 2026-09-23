namespace LINGYUN.Abp.Auditing.AuditLogs;

public class EntityChangeGetWithUsernameDto
{
    public string EntityId { get; set; } = default!;
    public string EntityTypeFullName { get; set; } = default!;
}

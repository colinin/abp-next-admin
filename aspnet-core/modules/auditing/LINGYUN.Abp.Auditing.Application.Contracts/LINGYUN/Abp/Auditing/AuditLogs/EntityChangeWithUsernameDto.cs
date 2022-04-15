namespace LINGYUN.Abp.Auditing.AuditLogs;
public class EntityChangeWithUsernameDto
{
    public EntityChangeDto EntityChange { get; set; }

    public string UserName { get; set; }
}

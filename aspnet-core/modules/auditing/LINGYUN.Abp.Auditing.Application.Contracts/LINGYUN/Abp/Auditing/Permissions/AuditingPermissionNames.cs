namespace LINGYUN.Abp.Auditing.Permissions;

public class AuditingPermissionNames
{
    public const string GroupName = "AbpAuditing";
    public class AuditLog
    {
        public const string Default = GroupName + ".AuditLog";
        public const string Delete = Default + ".Delete";
    }

    public class SecurityLog
    {
        public const string Default = GroupName + ".SecurityLog";
        public const string Delete = Default + ".Delete";
    }

    public class SystemLog
    {
        public const string Default = GroupName + ".SystemLog";
    }
}

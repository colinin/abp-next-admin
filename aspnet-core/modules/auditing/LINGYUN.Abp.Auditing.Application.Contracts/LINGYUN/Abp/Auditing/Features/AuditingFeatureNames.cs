namespace LINGYUN.Abp.Auditing.Features
{
    public static class AuditingFeatureNames
    {
        public const string GroupName = "AbpAuditing";
        public class AuditLog
        {
            public const string Default = GroupName + ".AuditLog";
        }

        public class SecurityLog
        {
            public const string Default = GroupName + ".SecurityLog";
        }
    }
}

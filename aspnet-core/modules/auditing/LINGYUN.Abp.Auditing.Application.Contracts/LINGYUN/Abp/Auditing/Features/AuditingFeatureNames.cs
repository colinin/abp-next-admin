namespace LINGYUN.Abp.Auditing.Features
{
    public static class AuditingFeatureNames
    {
        public const string GroupName = "AbpAuditing";
        public class Logging
        {
            public const string Default = GroupName + ".Logging";

            public const string AuditLog = Default + ".AuditLog";

            public const string SecurityLog = Default + ".SecurityLog";
        }
    }
}

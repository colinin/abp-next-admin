namespace LINGYUN.Abp.Auditing.Permissions
{
    public static class AuditingPermissionNames
    {
        public const string GroupName = "AbpAuditing";
        public static class AuditLog
        {
            public const string Default = GroupName + ".AuditLog";
            public const string Delete = Default + ".Delete";
        }

        public static class SecurityLog
        {
            public const string Default = GroupName + ".SecurityLog";
            public const string Delete = Default + ".Delete";
        }
    }
}

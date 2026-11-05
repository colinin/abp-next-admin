namespace LINGYUN.Abp.OperationRateLimiting;

public static class AbpOperationRateLimitingPermissions
{
    public const string GroupName = "OperationRateLimiting";

    public static class Policies
    {
        public const string Default = GroupName + ".Policies";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }
}

namespace LINGYUN.Abp.PermissionManagement.Permissions;
public static class PermissionManagementPermissionNames
{
    public const string GroupName = "PermissionManagement";

    public static class GroupDefinition
    {
        public const string Default = GroupName + ".GroupDefinitions";

        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }

    public static class Definition
    {
        public const string Default = GroupName + ".Definitions";

        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }
}

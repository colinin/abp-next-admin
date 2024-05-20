using Volo.Abp.Reflection;

namespace LINGYUN.Abp.DataProtectionManagement.Permissions;

public static class DataProtectionManagementPermissionNames
{
    public const string GroupName = "AbpDataProtectionManagement";

    public static class EntityTypeInfo
    {
        public const string Default = GroupName + ".EntityTypeInfo";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }

    public static class RoleEntityRule
    {
        public const string Default = GroupName + ".RoleEntityRule";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }

    public static class OrganizationUnitEntityRule
    {
        public const string Default = GroupName + ".OrganizationUnitEntityRule";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(DataProtectionManagementPermissionNames));
    }
}

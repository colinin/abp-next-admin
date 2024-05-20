namespace LINGYUN.Abp.DataProtectionManagement;

public static class DataProtectionManagementErrorCodes
{
    public const string Namespace = "DataProtectionManagement";

    public static class EntityTypeInfo
    {
        public const string Prefix = Namespace + ":001";
        /// <summary>
        /// 已经存在相同的类型
        /// </summary>
        public const string DuplicateTypeInfo = Prefix + "100";
        /// <summary>
        /// 已经存在相同的属性
        /// </summary>
        public const string DuplicateProperty = Prefix + "200";
    }

    public static class RoleEntityRule
    {
        public const string Prefix = Namespace + ":002";
        /// <summary>
        /// 已经存在相同的实体数据访问规则
        /// </summary>
        public const string DuplicateEntityRule = Prefix + "100";
    }

    public static class OrganizationUnitEntityRule
    {
        public const string Prefix = Namespace + ":003";
        /// <summary>
        /// 已经存在相同的实体数据访问规则
        /// </summary>
        public const string DuplicateEntityRule = Prefix + "100";
    }
}

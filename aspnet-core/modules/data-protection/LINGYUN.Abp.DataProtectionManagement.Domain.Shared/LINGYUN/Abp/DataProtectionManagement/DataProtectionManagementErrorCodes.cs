namespace LINGYUN.Abp.DataProtectionManagement;

public static class DataProtectionManagementErrorCodes
{
    public const string Namespace = "DataProtectionManagement";

    public static class EntityTypeInfo
    {
        public const string Prefix = Namespace + ":001";
        /// <summary>
        /// 已经存在名为 {Name} 的实体类型定义
        /// </summary>
        public const string DuplicateTypeInfo = Prefix + "100";
        /// <summary>
        /// 实体类型已经存在名为 {Name} 的属性定义
        /// </summary>
        public const string DuplicateProperty = Prefix + "200";
    }

    public static class RoleEntityRule
    {
        public const string Prefix = Namespace + ":002";
        /// <summary>
        /// 已为角色 {RoleName} 分配了实体 {Name} 的 {Operation} 访问规则!
        /// </summary>
        public const string DuplicateEntityRule = Prefix + "100";
    }

    public static class OrganizationUnitEntityRule
    {
        public const string Prefix = Namespace + ":003";
        /// <summary>
        /// 已为组织机构 {OrgCode} 分配了实体 {Name} 的 {Operation} 访问规则!
        /// </summary>
        public const string DuplicateEntityRule = Prefix + "100";
    }
}

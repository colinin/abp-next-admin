namespace LINGYUN.Abp.Saas;

public static class AbpSaasErrorCodes
{
    public const string Namespace = "Saas";
    /// <summary>
    /// 已经存在名为 {DisplayName} 的版本!
    /// </summary>
    public const string DuplicateEditionDisplayName = Namespace + ":010001";
    /// <summary>
    /// 试图删除正在使用的版本: {DisplayName}!
    /// </summary>
    public const string DeleteUsedEdition = Namespace + ":010002";
    /// <summary>
    /// 已经存在名为 {Name} 的租户!
    /// </summary>
    public const string DuplicateTenantName = Namespace + ":020001";
    /// <summary>
    /// 租户: {Tenant} 不存在!
    /// </summary>
    public const string TenantIdOrNameNotFound = Namespace + ":020002";
    /// <summary>
    /// 无效的默认连接字符串
    /// </summary>
    public const string InvalidDefaultConnectionString = Namespace + ":020101";
    /// <summary>
    /// 默认连接字符串指向的数据库已经存在
    /// </summary>
    public const string DefaultConnectionStringDatabaseExists = Namespace + ":020102";
    /// <summary>
    /// {Name} 的连接字符串无效
    /// </summary>
    public const string InvalidConnectionString = Namespace + ":020103";
}

namespace LINGYUN.Abp.Saas;

public static class AbpSaasErrorCodes
{
    public const string Namespace = "Saas";

    public const string DuplicateEditionDisplayName = Namespace + ":010001";
    public const string DeleteUsedEdition = Namespace + ":010002";
    public const string DuplicateTenantName = Namespace + ":020001";
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

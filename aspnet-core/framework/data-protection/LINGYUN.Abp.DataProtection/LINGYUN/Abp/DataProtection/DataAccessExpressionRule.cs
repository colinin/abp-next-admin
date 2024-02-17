namespace LINGYUN.Abp.DataProtection;
/// <summary>
/// 数据访问控制规则：自定义表达式
/// </summary>
public class DataAccessExpressionRule
{
    /// <summary>
    /// 表达式
    /// </summary>
    public virtual string Expression { get; protected set; }
}

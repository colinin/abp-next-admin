namespace LINGYUN.Abp.DataProtection;
/// <summary>
/// 数据访问控制规则：字段
/// </summary>
public class DataAccessFiledRule
{
    /// <summary>
    /// 字段名称
    /// </summary>
    public virtual string Field { get; protected set; }
    public DataAccessFiledRule()
    {
    }

    public DataAccessFiledRule(string field)
    {
        Field = field;
    }
}

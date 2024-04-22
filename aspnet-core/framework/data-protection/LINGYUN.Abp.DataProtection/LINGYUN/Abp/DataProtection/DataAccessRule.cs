using System.Collections.Generic;

namespace LINGYUN.Abp.DataProtection;
/// <summary>
/// 数据访问控制规则
/// </summary>
public class DataAccessRule
{
    /// <summary>
    /// 实体类型全名
    /// </summary>
    public virtual string EntityTypeFullName { get; protected set; }
    /// <summary>
    /// 数据访问操作
    /// </summary>
    public virtual DataAccessOperation Operation { get; protected set; }
    /// <summary>
    /// 数据访问角色
    /// </summary>
    public virtual DataAccessRole Role { get; protected set; }
    /// <summary>
    /// 资源访问者
    /// </summary>
    public virtual string Visitor { get; protected set; }
    /// <summary>
    /// 字段访问控制
    /// </summary>
    public virtual List<DataAccessFiledRule> Fileds { get; protected set; }
    /// <summary>
    /// 自定义表达式
    /// </summary>
    public virtual List<DataAccessExpressionRule> Expressions { get; protected set; }
    protected DataAccessRule()
    {
        Fileds = [];
        Expressions = [];
    }

    public DataAccessRule(
        string entityTypeFullName, 
        DataAccessOperation operation = DataAccessOperation.Read, 
        DataAccessRole role = DataAccessRole.All, 
        string visitor = null, 
        List<DataAccessFiledRule> fileds = null, 
        List<DataAccessExpressionRule> expressions = null)
    {
        EntityTypeFullName = entityTypeFullName;
        Operation = operation;
        Role = role;
        Visitor = visitor;
        Fileds = fileds ?? [];
        Expressions = expressions ?? [];
    }
}

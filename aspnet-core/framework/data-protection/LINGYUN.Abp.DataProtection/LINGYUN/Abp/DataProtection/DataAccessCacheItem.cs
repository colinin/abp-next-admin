using System.Collections.Generic;

namespace LINGYUN.Abp.DataProtection;
public class DataAccessCacheItem
{
    /// <summary>
    /// 实体类型全名
    /// </summary>
    public string EntityTypeFullName { get; set; }
    /// <summary>
    /// 数据访问操作
    /// </summary>
    public DataAccessOperation Operation { get; set; }
    /// <summary>
    /// 数据访问角色
    /// </summary>
    public DataAccessRole Role { get; set; }
    /// <summary>
    /// 资源访问者
    /// </summary>
    public string Visitor { get; set; }
    /// <summary>
    /// 字段访问控制
    /// </summary>
    public List<DataAccessFiledRule> Fileds { get; set; }
    /// <summary>
    /// 自定义表达式
    /// </summary>
    public List<DataAccessExpressionRule> Expressions { get;  set; }
}

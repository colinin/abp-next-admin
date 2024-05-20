using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.DataProtection;

[Serializable]
public class DataAccessResource
{
    /// <summary>
    /// 权限主体
    /// </summary>
    public string SubjectName { get; set; }

    /// <summary>
    /// 权限主体标识
    /// </summary>
    public string SubjectId { get; set; }

    /// <summary>
    /// 实体类型全名
    /// </summary>
    public string EntityTypeFullName { get; set; }

    /// <summary>
    /// 数据权限操作
    /// </summary>
    public DataAccessOperation Operation { get; set; }

    /// <summary>
    /// 获取或设置 数据过滤规则
    /// </summary>
    public DataAccessFilterGroup FilterGroup { get; set; }

    /// <summary>
    /// 允许操作的属性列表
    /// </summary>
    public List<string> AllowProperties { get; set; }

    public DataAccessResource()
    {

    }

    public DataAccessResource(
        string subjectName,
        string subjectId,
        string entityTypeFullName,
        DataAccessOperation operation,
        DataAccessFilterGroup filterGroup = null)
    {
        SubjectName = subjectName;
        SubjectId = subjectId;
        EntityTypeFullName = entityTypeFullName;
        Operation = operation;
        FilterGroup = filterGroup;
        AllowProperties = new List<string>();
    }
}

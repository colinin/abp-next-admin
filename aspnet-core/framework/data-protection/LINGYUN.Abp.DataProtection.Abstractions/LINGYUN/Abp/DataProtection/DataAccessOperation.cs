using System.ComponentModel;

namespace LINGYUN.Abp.DataProtection;
/// <summary>
/// 数据操作
/// </summary>
public enum DataAccessOperation
{
    /// <summary>
    /// 查询
    /// </summary>
    [Description("查询")]
    Read, 
    /// <summary>
    /// 更新
    /// </summary>
    [Description("更新")]
    Write, 
    /// <summary>
    /// 删除
    /// </summary>
    [Description("删除")]
    Delete
}

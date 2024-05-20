using System.ComponentModel;

namespace LINGYUN.Abp.DataProtection;
/// <summary>
/// 数据过滤操作
/// </summary>
public enum DataAccessFilterOperate
{
    /// <summary>
    /// 且
    /// </summary>
    [Description("且")]
    And = 1,
    /// <summary>
    /// 或
    /// </summary>
    [Description("或")]
    Or = 2,
    /// <summary>
    /// 等于
    /// </summary>
    [Description("等于")]
    Equal = 3,
    /// <summary>
    /// 不等于
    /// </summary>
    [Description("不等于")]
    NotEqual = 4,
    /// <summary>
    /// 小于
    /// </summary>
    [Description("小于")]
    Less = 5,
    /// <summary>
    /// 小于或等于
    /// </summary>
    [Description("小于等于")]
    LessOrEqual = 6,
    /// <summary>
    /// 大于
    /// </summary>
    [Description("大于")]
    Greater = 7,
    /// <summary>
    /// 大于或等于
    /// </summary>
    [Description("大于等于")]
    GreaterOrEqual = 8,
    /// <summary>
    /// 左包含
    /// </summary>
    [Description("左包含")]
    StartsWith = 9,
    /// <summary>
    /// 右包含
    /// </summary>
    [Description("右包含")]
    EndsWith = 10,
    /// <summary>
    /// 包含
    /// </summary>
    [Description("包含")]
    Contains = 11,
    /// <summary>
    /// 不包含
    /// </summary>
    [Description("不包含")]
    NotContains = 12,
}
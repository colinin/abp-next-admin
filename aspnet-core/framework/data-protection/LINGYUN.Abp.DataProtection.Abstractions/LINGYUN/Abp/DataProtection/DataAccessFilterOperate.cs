using System.ComponentModel;

namespace LINGYUN.Abp.DataProtection;
/// <summary>
/// 数据过滤操作
/// </summary>
public enum DataAccessFilterOperate
{
    /// <summary>
    /// 等于
    /// </summary>
    [Description("等于")]
    Equal = 1,
    /// <summary>
    /// 不等于
    /// </summary>
    [Description("不等于")]
    NotEqual = 2,
    /// <summary>
    /// 小于
    /// </summary>
    [Description("小于")]
    Less = 3,
    /// <summary>
    /// 小于或等于
    /// </summary>
    [Description("小于等于")]
    LessOrEqual = 4,
    /// <summary>
    /// 大于
    /// </summary>
    [Description("大于")]
    Greater = 5,
    /// <summary>
    /// 大于或等于
    /// </summary>
    [Description("大于等于")]
    GreaterOrEqual = 6,
    /// <summary>
    /// 左包含
    /// </summary>
    [Description("左包含")]
    StartsWith = 7,
    /// <summary>
    /// 右包含
    /// </summary>
    [Description("右包含")]
    EndsWith = 8,
    /// <summary>
    /// 包含
    /// </summary>
    [Description("包含")]
    Contains = 9,
    /// <summary>
    /// 不包含
    /// </summary>
    [Description("不包含")]
    NotContains = 10,
}
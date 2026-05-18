using System;

namespace LINGYUN.Abp.Calendar;
/// <summary>
/// 特殊日期
/// </summary>
public class SpecialDateInfo
{
    /// <summary>
    /// 日期
    /// </summary>
    public DateOnly Date { get; set; }
    /// <summary>
    /// 特殊日期类型
    /// </summary>
    public SpecialDateType Type { get; set; }
    /// <summary>
    /// 特殊日期描述
    /// </summary>
    public string? Description { get; set; }
}

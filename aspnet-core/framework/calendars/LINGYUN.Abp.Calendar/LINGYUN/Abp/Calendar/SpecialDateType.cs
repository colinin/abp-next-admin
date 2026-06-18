using System.ComponentModel;

namespace LINGYUN.Abp.Calendar;
/// <summary>
/// 特殊日期类型
/// </summary>
public enum SpecialDateType
{
    /// <summary>
    /// 法定节假日
    /// </summary>
    [Description("法定节假日")]
    Holiday = 0,
    /// <summary>
    /// 特殊工作日
    /// </summary>
    [Description("特殊工作日")]
    SpecialWorkday = 1,
    /// <summary>
    /// 自定义假期
    /// </summary>
    [Description("自定义假期")]
    CustomHoliday = 2
}

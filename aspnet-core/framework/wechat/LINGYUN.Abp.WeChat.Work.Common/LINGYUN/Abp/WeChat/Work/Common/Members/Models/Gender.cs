using System.ComponentModel;

namespace LINGYUN.Abp.WeChat.Work.Common.Members.Models;
/// <summary>
/// 性别
/// </summary>
[Description("性别")]
public enum Gender
{
    /// <summary>
    /// 未知
    /// </summary>
    [Description("未知")]
    None = 0,
    /// <summary>
    /// 男性
    /// </summary>
    [Description("男性")]
    Male = 1,
    /// <summary>
    /// 女性
    /// </summary>
    [Description("女性")]
    FeMale = 2,
}
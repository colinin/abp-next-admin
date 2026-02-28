using System.ComponentModel;

namespace LINGYUN.Abp.WeChat.Work.Contacts.Members.Models;
/// <summary>
/// 二维码尺寸类型
/// </summary>
[Description("二维码尺寸类型")]
public enum QrCodeSizeType
{
    /// <summary>
    /// 171 x 171
    /// </summary>
    [Description("171 x 171")]
    Size171 = 1,
    /// <summary>
    /// 399 x 399
    /// </summary>
    [Description("399 x 399")]
    Size399 = 2,
    /// <summary>
    /// 741 x 741
    /// </summary>
    [Description("741 x 741")]
    Size741 = 3,
    /// <summary>
    /// 2052 x 2052
    /// </summary>
    [Description("2052 x 2052")]
    Size2052 = 4,
}

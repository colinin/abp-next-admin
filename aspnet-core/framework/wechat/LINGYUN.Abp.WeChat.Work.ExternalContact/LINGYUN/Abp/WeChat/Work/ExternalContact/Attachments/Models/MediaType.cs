using System.ComponentModel;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Attachments.Models;
/// <summary>
/// 媒体文件类型
/// </summary>
[Description("媒体文件类型")]
public enum MediaType
{
    /// <summary>
    /// 图片
    /// </summary>
    [Description("图片")]
    Image = 1,
    /// <summary>
    /// 视频
    /// </summary>
    [Description("视频")]
    Video = 2,
    /// <summary>
    /// 普通文件
    /// </summary>
    [Description("普通文件")]
    File = 3,
}

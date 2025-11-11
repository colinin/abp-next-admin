using System.ComponentModel;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Attachments.Models;
/// <summary>
/// 附件类型
/// </summary>
[Description("附件类型")]
public enum AttachmentType
{
    /// <summary>
    /// 朋友圈
    /// </summary>
    [Description("朋友圈")]
    Moments = 1,
    /// <summary>
    /// 商品图册
    /// </summary>
    [Description("商品图册")]
    ProductCatalogue = 2,
}

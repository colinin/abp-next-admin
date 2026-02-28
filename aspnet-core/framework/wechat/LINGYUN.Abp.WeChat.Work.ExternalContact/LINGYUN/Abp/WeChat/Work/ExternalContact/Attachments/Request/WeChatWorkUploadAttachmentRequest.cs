using JetBrains.Annotations;
using LINGYUN.Abp.WeChat.Work.ExternalContact.Attachments.Models;
using Volo.Abp;
using Volo.Abp.Content;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Attachments.Request;
/// <summary>
/// 上传附件资源请求参数
/// </summary>
/// <remarks>
/// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/95098" />
/// </remarks>
public class WeChatWorkUploadAttachmentRequest : WeChatWorkRequest
{
    /// <summary>
    /// 媒体文件类型
    /// </summary>
    [NotNull]
    public MediaType MediaType { get; }
    /// <summary>
    /// 附件类型
    /// </summary>
    [NotNull]
    public AttachmentType AttachmentType { get; }
    /// <summary>
    /// 媒体文件
    /// </summary>
    [NotNull]
    public IRemoteStreamContent Content { get; }
    public WeChatWorkUploadAttachmentRequest(
        MediaType mediaType, 
        AttachmentType attachmentType,
        IRemoteStreamContent content)
    {
        Check.NotNull(content, nameof(content));

        MediaType = mediaType;
        AttachmentType = attachmentType;
        Content = content;
    }
}

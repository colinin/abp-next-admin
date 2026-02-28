using LINGYUN.Abp.WeChat.Work.ExternalContact.Attachments.Request;
using LINGYUN.Abp.WeChat.Work.ExternalContact.Attachments.Response;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Attachments;
/// <summary>
/// 附件资源接口
/// </summary>
public interface IWeChatWorkAttachmentProvider
{
    /// <summary>
    /// 上传附件资源
    /// </summary>
    /// <remarks>
    /// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/90253"/>
    /// </remarks>
    /// <param name="request">请求参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkUploadAttachmentResponse> UploadAsync(
        WeChatWorkUploadAttachmentRequest request,
        CancellationToken cancellationToken = default);
}

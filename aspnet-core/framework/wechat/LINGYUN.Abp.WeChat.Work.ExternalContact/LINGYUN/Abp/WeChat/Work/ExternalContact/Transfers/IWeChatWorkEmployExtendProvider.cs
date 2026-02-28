using LINGYUN.Abp.WeChat.Work.ExternalContact.Transfers.Request;
using LINGYUN.Abp.WeChat.Work.ExternalContact.Transfers.Response;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Transfers;
/// <summary>
/// 在职继承接口
/// </summary>
public interface IWeChatWorkEmployExtendProvider
{
    /// <summary>
    /// 分配在职成员的客户
    /// </summary>
    /// <remarks>
    /// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/92124"/>
    /// </remarks>
    /// <param name="request">请求参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkTransferCustomerResponse> TransferCustomerAsync(
        WeChatWorkTransferCustomerRequest request,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 查询客户接替状态
    /// </summary>
    /// <remarks>
    /// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/94088"/>
    /// </remarks>
    /// <param name="request">请求参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkGetTransferResultResponse> GetTransferResultAsync(
        WeChatWorkGetTransferResultRequest request,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 分配在职成员的客户群
    /// </summary>
    /// <remarks>
    /// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/95703"/>
    /// </remarks>
    /// <param name="request">请求参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkGroupChatOnjobTransferResponse> GroupChatOnjobTransferAsync(
        WeChatWorkGroupChatOnjobTransferRequest request,
        CancellationToken cancellationToken = default);
}

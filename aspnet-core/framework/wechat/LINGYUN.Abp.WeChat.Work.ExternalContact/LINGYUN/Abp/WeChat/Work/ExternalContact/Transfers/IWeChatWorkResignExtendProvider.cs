using LINGYUN.Abp.WeChat.Work.ExternalContact.Transfers.Request;
using LINGYUN.Abp.WeChat.Work.ExternalContact.Transfers.Response;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Transfers;
/// <summary>
/// 离职继承接口
/// </summary>
public interface IWeChatWorkResignExtendProvider
{
    /// <summary>
    /// 获取待分配的离职成员列表
    /// </summary>
    /// <remarks>
    /// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/92124"/>
    /// </remarks>
    /// <param name="request">请求参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkGetUnassignedListResponse> GetUnassignedListAsync(
        WeChatWorkGetUnassignedListRequest request,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 分配离职成员的客户
    /// </summary>
    /// <remarks>
    /// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/94081"/>
    /// </remarks>
    /// <param name="request">请求参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkResignedTransferCustomerResponse> ResignedTransferCustomerAsync(
        WeChatWorkResignedTransferCustomerRequest request,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 查询客户接替状态
    /// </summary>
    /// <remarks>
    /// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/94082"/>
    /// </remarks>
    /// <param name="request">请求参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkGetResignedTransferResultResponse> GetResignedTransferResultAsync(
        WeChatWorkGetResignedTransferResultRequest request,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 分配离职成员的客户群
    /// </summary>
    /// <remarks>
    /// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/92127"/>
    /// </remarks>
    /// <param name="request">请求参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkGroupChatTransferResponse> GroupChatTransferAsync(
        WeChatWorkGroupChatTransferRequest request,
        CancellationToken cancellationToken = default);
}

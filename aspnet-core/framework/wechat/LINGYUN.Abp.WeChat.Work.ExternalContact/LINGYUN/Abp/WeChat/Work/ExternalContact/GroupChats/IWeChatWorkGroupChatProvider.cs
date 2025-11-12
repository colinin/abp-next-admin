using LINGYUN.Abp.WeChat.Work.ExternalContact.GroupChats.Request;
using LINGYUN.Abp.WeChat.Work.ExternalContact.GroupChats.Response;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.GroupChats;
/// <summary>
/// 客户群管理接口
/// </summary>
public interface IWeChatWorkGroupChatProvider
{
    /// <summary>
    /// 获取客户群列表
    /// </summary>
    /// <remarks>
    /// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/92120" />
    /// </remarks>
    /// <param name="request">请求参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkGetGroupChatListResponse> GetGroupChatListAsync(
        WeChatWorkGetGroupChatListRequest request,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 获取客户群详情
    /// </summary>
    /// <remarks>
    /// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/92122" />
    /// </remarks>
    /// <param name="request">请求参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkGetGroupChatResponse> GetGroupChatAsync(
        WeChatWorkGetGroupChatRequest request,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 客户群opengid转换
    /// </summary>
    /// <remarks>
    /// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/94822" />
    /// </remarks>
    /// <param name="request">请求参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkOpengIdToChatIdResponse> OpengIdToChatIdAsync(
        WeChatWorkOpengIdToChatIdRequest request,
        CancellationToken cancellationToken = default);
}

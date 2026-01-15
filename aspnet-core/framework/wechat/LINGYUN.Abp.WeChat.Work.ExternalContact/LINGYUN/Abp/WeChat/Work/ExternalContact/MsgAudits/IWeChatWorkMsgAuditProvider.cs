using LINGYUN.Abp.WeChat.Work.ExternalContact.MsgAudits.Request;
using LINGYUN.Abp.WeChat.Work.ExternalContact.MsgAudits.Response;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.MsgAudits;
/// <summary>
/// 会话存档接口
/// </summary>
public interface IWeChatWorkMsgAuditProvider
{
    /// <summary>
    /// 获取会话内容存档开启成员列表
    /// </summary>
    /// <remarks>
    /// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/91614" />
    /// </remarks>
    Task<WeChatWorkGetPermitUserListResponse> GetPermitUserListAsync(
        WeChatWorkGetPermitUserListRequest request,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 获取单聊会话同意情况
    /// </summary>
    /// <remarks>
    /// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/91782" />
    /// </remarks>
    Task<WeChatWorkCheckSingleAgreeResponse> CheckSingleAgreeAsync(
        WeChatWorkCheckSingleAgreeRequest request,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 获取群聊会话同意情况
    /// </summary>
    /// <remarks>
    /// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/91782" />
    /// </remarks>
    Task<WeChatWorkCheckRoomAgreeResponse> CheckRoomAgreeAsync(
        WeChatWorkCheckRoomAgreeRequest request,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 获取会话内容存档内部群信息
    /// </summary>
    /// <remarks>
    /// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/92951" />
    /// </remarks>
    Task<WeChatWorkGetGroupChatResponse> GetGroupChatAsync(
        WeChatWorkGetGroupChatRequest request,
        CancellationToken cancellationToken = default);
}

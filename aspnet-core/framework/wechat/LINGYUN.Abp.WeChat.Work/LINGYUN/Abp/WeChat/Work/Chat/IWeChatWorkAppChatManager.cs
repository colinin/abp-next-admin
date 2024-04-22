using LINGYUN.Abp.WeChat.Work.Chat.Request;
using LINGYUN.Abp.WeChat.Work.Chat.Response;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.Work.Chat;
/// <summary>
/// 群聊操作接口
/// </summary>
/// <remarks>
/// 参考：https://developer.work.weixin.qq.com/document/path/90244
/// </remarks>
public interface IWeChatWorkAppChatManager
{
    /// <summary>
    /// 创建群聊会话
    /// </summary>
    /// <remarks>
    /// 参考：https://developer.work.weixin.qq.com/document/path/90245
    /// </remarks>
    Task<WeChatWorkAppChatCreateResponse> CreateAsync(
        WeChatWorkAppChatCreateRequest request, 
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 修改群聊会话
    /// </summary>
    /// <remarks>
    /// 参考：https://developer.work.weixin.qq.com/document/path/98913
    /// </remarks>
    Task<WeChatWorkResponse> UpdateAsync(
        WeChatWorkAppChatUpdateRequest request,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 获取群聊会话
    /// </summary>
    /// <remarks>
    /// 参考：https://developer.work.weixin.qq.com/document/path/98914
    /// </remarks>
    Task<WeChatWorkAppChatInfoResponse> GetAsync(
        string agentId,
        string chatId,
        CancellationToken cancellationToken = default);
}

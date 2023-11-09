using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.Work.Messages;
/// <summary>
/// 企业微信消息管理器
/// </summary>
public interface IWeChatWorkMessageManager
{
    /// <summary>
    /// 撤回应用消息
    /// </summary>
    /// <remarks>
    /// 参考：https://developer.work.weixin.qq.com/document/path/94867
    /// </remarks>
    /// <param name="agentId">应用标识</param>
    /// <param name="messageId">消息ID。从应用发送消息接口 <see cref="IWeChatWorkMessageSender"/> 处获得。</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> ReCallMessageAsync(string agentId, string messageId, CancellationToken cancellationToken = default);

}

using LINGYUN.Abp.WeChat.Work.Messages.Response;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.Work.Messages;
/// <summary>
/// 消息发送接口
/// </summary>
public interface IWeChatWorkMessageSender
{
    /// <summary>
    /// 发送应用消息
    /// </summary>
    /// <remarks>
    /// 参考：https://developer.work.weixin.qq.com/document/path/90236
    /// </remarks>
    /// <param name="message">继承自 <see cref="WeChatWorkMessage"/> 的企业微信消息载体</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkMessageResponse> SendAsync(
        WeChatWorkMessage message,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 发送群聊消息
    /// </summary>
    /// <remarks>
    /// 参考：https://developer.work.weixin.qq.com/document/path/90248
    /// </remarks>
    /// <param name="message">继承自 <see cref="WeChatWorkAppChatMessage"/> 的企业微信群聊消息载体</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkResponse> SendAsync(
        WeChatWorkAppChatMessage message,
        CancellationToken cancellationToken = default);
}

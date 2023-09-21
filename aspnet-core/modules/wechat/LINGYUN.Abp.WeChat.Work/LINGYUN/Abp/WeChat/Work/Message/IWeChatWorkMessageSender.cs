using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.Work.Message;
/// <summary>
/// 消息发送接口
/// </summary>
public interface IWeChatWorkMessageSender
{
    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="message">继承自 <see cref="WeChatWorkMessage"/> 的企业微信消息载体</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkMessageResponse> SendAsync(
        WeChatWorkMessage message,
        CancellationToken cancellationToken = default);
}

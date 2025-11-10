using LINGYUN.Abp.WeChat.Common.Messages;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.Official.Messages;
/// <summary>
/// 微信公众号消息解析器
/// </summary>
public interface IWeChatOfficialMessageResolver
{
    /// <summary>
    /// 解析微信公众号消息
    /// </summary>
    /// <param name="messageData">公众号消息</param>
    /// <returns></returns>
    Task<MessageResolveResult> ResolveAsync(MessageResolveData messageData);
}

using LINGYUN.Abp.WeChat.Common.Messages;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.Work.Common.Messages;
/// <summary>
/// 企业微信消息解析器
/// </summary>
public interface IWeChatWorkMessageResolver
{
    /// <summary>
    /// 解析企业微信消息
    /// </summary>
    /// <param name="messageData">企业微信消息</param>
    /// <returns></returns>
    Task<MessageResolveResult> ResolveAsync(MessageResolveData messageData);
}

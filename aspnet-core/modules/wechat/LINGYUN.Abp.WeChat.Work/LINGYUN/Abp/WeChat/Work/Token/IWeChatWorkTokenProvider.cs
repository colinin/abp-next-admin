using LINGYUN.Abp.WeChat.Work.Token.Models;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.Work.Token;
/// <summary>
/// 企业微信AccessToken接口
/// </summary>
public interface IWeChatWorkTokenProvider
{
    /// <summary>
    /// 获取应用Token
    /// </summary>
    /// <remarks>
    /// 参考：https://developer.work.weixin.qq.com/document/path/91039
    /// </remarks>
    /// <param name="agentId">应用标识</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkToken> GetTokenAsync(string agentId, CancellationToken cancellationToken = default);
    /// <summary>
    /// 获取应用Token
    /// </summary>
    /// <remarks>
    /// 参考：https://developer.work.weixin.qq.com/document/path/91039
    /// </remarks>
    /// <param name="corpId">企业标识</param>
    /// <param name="agentId">应用标识</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkToken> GetTokenAsync(string corpId, string agentId, CancellationToken cancellationToken = default);
}

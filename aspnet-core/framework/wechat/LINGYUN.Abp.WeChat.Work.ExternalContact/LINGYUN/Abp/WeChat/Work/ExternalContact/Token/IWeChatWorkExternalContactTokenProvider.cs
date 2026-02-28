using LINGYUN.Abp.WeChat.Work.Token.Models;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Token;
/// <summary>
/// 客户联系应用Token提供者
/// </summary>
/// <remarks>
/// 企业微信部分接口需要使用客户联系应用Token
/// </remarks>
public interface IWeChatWorkExternalContactTokenProvider
{
    /// <summary>
    /// 获取客户联系应用Token
    /// </summary>
    /// <remarks>
    /// 参考：https://developer.work.weixin.qq.com/document/path/91039
    /// </remarks>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkToken> GetTokenAsync(CancellationToken cancellationToken = default);
}

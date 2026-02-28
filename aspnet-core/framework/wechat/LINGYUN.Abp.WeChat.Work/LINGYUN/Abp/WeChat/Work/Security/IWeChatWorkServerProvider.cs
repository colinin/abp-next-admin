using LINGYUN.Abp.WeChat.Work.Security.Models;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.Work.Security;
public interface IWeChatWorkServerProvider
{
    /// <summary>
    /// 获取企业微信域名IP信息
    /// </summary>
    /// <remarks>
    /// 参考：https://developer.work.weixin.qq.com/document/path/100079
    /// </remarks>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatServerDomainResponse> GetWeChatServerAsync(CancellationToken cancellationToken = default);
}

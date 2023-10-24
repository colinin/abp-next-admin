using LINGYUN.Abp.WeChat.Work.Authorize.Models;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.Work.Authorize;
/// <summary>
/// 企业微信用户信息查询接口
/// </summary>
public interface IWeChatWorkUserFinder
{
    Task<WeChatWorkUserInfo> GetUserInfoAsync(
        string agentId,
        string code,
        CancellationToken cancellationToken = default);

    Task<WeChatWorkUserDetail> GetUserDetailAsync(
        string agentId,
        string userTicket,
        CancellationToken cancellationToken = default);
}

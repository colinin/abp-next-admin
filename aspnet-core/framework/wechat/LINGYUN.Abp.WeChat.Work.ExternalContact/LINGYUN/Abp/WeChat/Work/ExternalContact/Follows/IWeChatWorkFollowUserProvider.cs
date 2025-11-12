using LINGYUN.Abp.WeChat.Work.ExternalContact.Follows.Response;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Follows;
/// <summary>
/// 企业服务人员管理接口
/// </summary>
public interface IWeChatWorkFollowUserProvider
{
    /// <summary>
    /// 获取配置了客户联系功能的成员列表
    /// </summary>
    /// <remarks>
    /// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/92571" />
    /// </remarks>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkGetFollowUserListResponse> GetFollowUserListAsync(CancellationToken cancellationToken = default);
}

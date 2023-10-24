using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WxPusher.User;
/// <summary>
/// 用户接口
/// </summary>
public interface IWxPusherUserProvider
{
    /// <summary>
    /// 查询App的关注用户V2
    /// </summary>
    /// <remarks>
    /// 获取到所有关注应用/主题的微信用户用户信息。需要注意，一个微信用户，如果同时关注应用，主题，甚至关注多个主题，会返回多条记录。
    /// </remarks>
    /// <param name="page">请求数据的页码</param>
    /// <param name="pageSize">分页大小，不能超过100</param>
    /// <param name="uid">用户的uid，可选，如果不传就是查询所有用户，传uid就是查某个用户的信息。</param>
    /// <param name="isBlock">查询拉黑用户，可选，不传查询所有用户，true查询拉黑用户，false查询没有拉黑的用户</param>
    /// <param name="type">关注的类型，可选，不传查询所有用户，0是应用，1是主题</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WxPusherPagedResult<UserProfile>> GetUserListAsync(
        int page = 1,
        int pageSize = 10,
        string uid = null,
        bool? isBlock = null,
        FlowType? type = null,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 删除用户
    /// </summary>
    /// <remarks>
    /// 你可以删除用户对应用、主题的关注，删除以后，用户可以重新关注，如想让用户再次关注，可以调用拉黑接口，对用户拉黑。
    /// </remarks>
    /// <param name="id">用户id，通过用户查询接口可以获取</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> DeleteUserAsync(
        int id,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 拉黑用户
    /// </summary>
    /// <remarks>
    /// 拉黑以后不能再发送消息，用户也不能再次关注，除非你取消对他的拉黑。调用拉黑接口，不用再调用删除接口。
    /// </remarks>
    /// <param name="id">用户id，通过用户查询接口可以获取</param>
    /// <param name="reject">是否拉黑，true表示拉黑，false表示取消拉黑</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> RejectUserAsync(
        int id,
        bool reject,
        CancellationToken cancellationToken = default);
}

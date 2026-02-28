using LINGYUN.Abp.WeChat.Work.ExternalContact.Customers.Request;
using LINGYUN.Abp.WeChat.Work.ExternalContact.Customers.Response;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Customers;
/// <summary>
/// 客户管理接口
/// </summary>
public interface IWeChatWorkCustomerProvider
{
    /// <summary>
    /// 获取客户列表
    /// </summary>
    /// <remarks>
    /// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/92113" />
    /// </remarks>
    /// <param name="userId">企业成员的userid</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkGetCustomerListResponse> GetCustomerListAsync(
        string userId,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 批量获取客户详情
    /// </summary>
    /// <remarks>
    /// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/92994" />
    /// </remarks>
    /// <param name="request">请求参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkBulkGetCustomerResponse> BulkGetCustomerAsync(
        WeChatWorkBulkGetCustomerRequest request,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 获取客户详情
    /// </summary>
    /// <remarks>
    /// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/92114" />
    /// </remarks>
    /// <param name="externalUserid">外部联系人的userid，注意不是企业成员的账号</param>
    /// <param name="cursor">上次请求返回的next_cursor</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkGetCustomerResponse> GetCustomerAsync(
        string externalUserid, 
        string? cursor = null,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 修改客户备注信息
    /// </summary>
    /// <remarks>
    /// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/92115" />
    /// </remarks>
    /// <param name="request">请求参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkResponse> UpdateCustomerRemarkAsync(
        WeChatWorkUpdateCustomerRemarkRequest request,
        CancellationToken cancellationToken = default);
}

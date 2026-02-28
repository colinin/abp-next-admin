using LINGYUN.Abp.WeChat.Work.ExternalContact.Contacts.Request;
using LINGYUN.Abp.WeChat.Work.ExternalContact.Contacts.Response;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Contacts;
/// <summary>
/// 外部联系人接口
/// </summary>
public interface IWeChatWorkExternalContactProvider
{
    /// <summary>
    /// 获取已服务的外部联系人
    /// </summary>
    /// <remarks>
    /// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/99434" />
    /// </remarks>
    /// <param name="request">请求参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkGetExternalContactListResponse> GetExternalContactListAsync(
        WeChatWorkGetExternalContactListRequest request,
        CancellationToken cancellationToken = default);
}

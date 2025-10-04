using LINGYUN.Abp.WeChat.Work.JsSdk.Models;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.Work.JsSdk;
/// <summary>
/// JS-SDK临时票据提供者
/// See: https://developer.work.weixin.qq.com/document/path/90506
/// </summary>
public interface IJsApiTicketProvider
{
    /// <summary>
    /// 获取企业 jsapi_ticket
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<JsApiTicketInfo> GetTicketInfoAsync(CancellationToken cancellationToken = default);
    /// <summary>
    /// 获取应用 jsapi_ticket
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<JsApiTicketInfo> GetAgentTicketInfoAsync(CancellationToken cancellationToken = default);
    /// <summary>
    /// 获取JS-SDK签名
    /// </summary>
    /// <param name="ticketInfo">JS-SDK临时票据</param>
    /// <param name="url">生成签名的url</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    JsApiSignatureData GenerateSignature(JsApiTicketInfo ticketInfo, string url, CancellationToken cancellationToken = default);
}

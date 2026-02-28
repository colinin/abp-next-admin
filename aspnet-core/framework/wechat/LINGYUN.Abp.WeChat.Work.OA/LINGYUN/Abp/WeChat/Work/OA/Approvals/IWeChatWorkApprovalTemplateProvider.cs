using LINGYUN.Abp.WeChat.Work.OA.Approvals.Request;
using LINGYUN.Abp.WeChat.Work.OA.Approvals.Response;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.Work.OA.Approvals;
/// <summary>
/// 审批模板接口
/// </summary>
/// <remarks>
/// 详情见: https://developer.work.weixin.qq.com/document/path/91854
/// </remarks>
public interface IWeChatWorkApprovalTemplateProvider
{
    /// <summary>
    /// 创建审批模板
    /// </summary>
    /// <remarks>
    /// 详情见：https://developer.work.weixin.qq.com/document/path/97437
    /// </remarks>
    /// <param name="request">创建审批模板请求参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns>创建审批模板响应参数</returns>
    Task<WeChatWorkCreateTemplateResponse> CreateTemplateAsync(
        WeChatWorkCreateTemplateRequest request,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 更新审批模板
    /// </summary>
    /// <remarks>
    /// 详情见：https://developer.work.weixin.qq.com/document/path/97438
    /// </remarks>
    /// <param name="request">更新审批模板请求参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns>更新审批模板响应参数</returns>
    Task<WeChatWorkResponse> UpdateTemplateAsync(
        WeChatWorkCreateTemplateRequest request,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 获取审批模板详情
    /// </summary>
    /// <remarks>
    /// 详情见：https://developer.work.weixin.qq.com/document/path/91982
    /// </remarks>
    /// <param name="request">获取审批模板请求参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns>审批模板响应参数</returns>
    Task<WeChatWorkTemplateResponse> GetTemplateAsync(
        WeChatWorkGetTemplateRequest request,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 批量获取审批单号
    /// </summary>
    /// <remarks>
    /// 详情见：https://developer.work.weixin.qq.com/document/path/91816
    /// </remarks>
    /// <param name="request">批量获取审批单号请求参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns>批量获取审批单号响应参数</returns>
    Task<WeChatWorkGetApprovalInfoResponse> GetApprovalInfoAsync(
        WeChatWorkGetApprovalInfoRequest request,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 获取审批申请详情
    /// </summary>
    /// <remarks>
    /// 详情见：https://developer.work.weixin.qq.com/document/path/91983
    /// </remarks>
    /// <param name="request">获取审批申请详情请求参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns>获取审批申请详情响应参数</returns>
    Task<WeChatWorkGetApprovalDetailResponse> GetApprovalDetailAsync(
        WeChatWorkGetApprovalDetailRequest request,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 提交审批申请
    /// </summary>
    /// <remarks>
    /// 详情见：https://developer.work.weixin.qq.com/document/path/91853
    /// </remarks>
    /// <param name="request">提交审批申请请求参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns>提交审批申请响应参数</returns>
    Task<WeChatWorkApplyEventResponse> ApplyEventAsync(
        WeChatWorkApplyEventRequest request,
        CancellationToken cancellationToken = default);
}

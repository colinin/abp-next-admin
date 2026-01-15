using LINGYUN.Abp.WeChat.Work.Contacts.Departments.Request;
using LINGYUN.Abp.WeChat.Work.Contacts.Departments.Response;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.Work.Contacts.Departments;
/// <summary>
/// 企业微信部门管理接口
/// </summary>
public interface IWeChatWorkDepartmentProvider
{
    /// <summary>
    /// 获取部门列表
    /// </summary>
    /// <remarks>
    /// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/90208" />
    /// </remarks>
    /// <param name="request">请求参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkGetDepartmentListResponse> GetDepartmentListAsync(
        WeChatWorkGetDepartmentListRequest request,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 获取子部门ID列表
    /// </summary>
    /// <remarks>
    /// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/95350" />
    /// </remarks>
    /// <param name="request">请求参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkGetSubDepartmentListResponse> GetSubDepartmentListAsync(
        WeChatWorkGetSubDepartmentListRequest request,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 获取单个部门详情
    /// </summary>
    /// <remarks>
    /// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/95351" />
    /// </remarks>
    /// <param name="request">请求参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkGetDepartmentResponse> GetDepartmentAsync(
        WeChatWorkGetDepartmentRequest request,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 创建部门
    /// </summary>
    /// <remarks>
    /// 此接口使用通讯录应用Token<br />
    /// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/90205" />
    /// </remarks>
    /// <param name="request">请求参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkCreateDepartmentResponse> CreateDepartmentAsync(
        WeChatWorkCreateDepartmentRequest request,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 更新部门
    /// </summary>
    /// <remarks>
    /// 此接口使用通讯录应用Token<br />
    /// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/90206" />
    /// </remarks>
    /// <param name="request">请求参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkResponse> UpdateDepartmentAsync(
        WeChatWorkUpdateDepartmentRequest request,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 删除部门
    /// </summary>
    /// <remarks>
    /// 此接口使用通讯录应用Token<br />
    /// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/90207" />
    /// </remarks>
    /// <param name="request">请求参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkResponse> DeleteDepartmentAsync(
        WeChatWorkDeleteDepartmentRequest request,
        CancellationToken cancellationToken = default);
}

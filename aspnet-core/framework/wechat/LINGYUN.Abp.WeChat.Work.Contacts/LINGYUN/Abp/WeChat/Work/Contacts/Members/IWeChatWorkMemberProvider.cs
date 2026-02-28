using LINGYUN.Abp.WeChat.Work.Contacts.Members.Models;
using LINGYUN.Abp.WeChat.Work.Contacts.Members.Request;
using LINGYUN.Abp.WeChat.Work.Contacts.Members.Response;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.Work.Contacts.Members;
/// <summary>
/// 企业微信成员管理接口
/// </summary>
public interface IWeChatWorkMemberProvider
{
    /// <summary>
    /// 读取成员
    /// </summary>
    /// <remarks>
    /// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/90196" />
    /// </remarks>
    /// <param name="userId">成员UserID。对应管理端的账号，企业内必须唯一。不区分大小写，长度为1~64个字节</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkGetMemberResponse> GetMemberAsync(
        string userId,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 创建成员
    /// </summary>
    /// <remarks>
    /// 此接口使用通讯录应用Token<br />
    /// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/90195" />
    /// </remarks>
    /// <param name="request">请求参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkCreateMemberResponse> CreateMemberAsync(
        WeChatWorkCreateMemberRequest request,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 更新成员
    /// </summary>
    /// <remarks>
    /// 此接口使用通讯录应用Token<br />
    /// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/90197" />
    /// </remarks>
    /// <param name="request">请求参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkResponse> UpdateMemberAsync(
        WeChatWorkUpdateMemberRequest request,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 删除成员
    /// </summary>
    /// <remarks>
    /// 此接口使用通讯录应用Token<br />
    /// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/90198" />
    /// </remarks>
    /// <param name="userId">成员UserID。对应管理端的账号</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkResponse> DeleteMemberAsync(
        string userId,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 批量删除成员
    /// </summary>
    /// <remarks>
    /// 此接口使用通讯录应用Token<br />
    /// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/90199" />
    /// </remarks>
    /// <param name="request">请求参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkResponse> BulkDeleteMemberAsync(
        WeChatWorkBulkDeleteMemberRequest request,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 获取部门成员
    /// </summary>
    /// <remarks>
    /// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/90200" />
    /// </remarks>
    /// <param name="departmentId">获取的部门id</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkGetSimpleMemberListResponse> GetSimpleMemberListAsync(
        int departmentId,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 获取部门成员详情
    /// </summary>
    /// <remarks>
    /// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/90201" />
    /// </remarks>
    /// <param name="departmentId">获取的部门id</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkGetMemberListResponse> GetMemberListAsync(
        int departmentId,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// userid转openid
    /// </summary>
    /// <remarks>
    /// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/90202" />
    /// </remarks>
    /// <param name="request">请求参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkConvertToOpenIdResponse> ConvertToOpenIdAsync(
        WeChatWorkConvertToOpenIdRequest request,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// openid转userid
    /// </summary>
    /// <remarks>
    /// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/90202" />
    /// </remarks>
    /// <param name="request">请求参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkConvertToUserIdResponse> ConvertToUserIdAsync(
        WeChatWorkConvertToUserIdRequest request,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 登录二次验证
    /// </summary>
    /// <remarks>
    /// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/90203" />
    /// </remarks>
    /// <param name="userId">成员UserID。对应管理端的账号</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkResponse> AuthSuccessAsync(
        string userId,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 邀请成员
    /// </summary>
    /// <remarks>
    /// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/90975" />
    /// </remarks>
    /// <param name="request">请求参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkBulkInviteMemberResponse> BulkInviteMemberAsync(
        WeChatWorkBulkInviteMemberRequest request,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 获取加入企业二维码
    /// </summary>
    /// <remarks>
    /// 此接口使用通讯录应用Token<br />
    /// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/91714" />
    /// </remarks>
    /// <param name="sizeType">二维码尺寸类型</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkGetJoinQrCodeResponse> GetJoinQrCodeAsync(
        QrCodeSizeType sizeType,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 手机号获取userid
    /// </summary>
    /// <remarks>
    /// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/95402" />
    /// </remarks>
    /// <param name="request">请求参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkGetUserIdResponse> GetUserIdByMobileAsync(
        WeChatWorkGetUserIdByMobileRequest request,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 邮箱获取userid
    /// </summary>
    /// <remarks>
    /// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/95895" />
    /// </remarks>
    /// <param name="request">请求参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkGetUserIdResponse> GetUserIdByEmailAsync(
        WeChatWorkGetUserIdByEmailRequest request,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 获取成员ID列表
    /// </summary>
    /// <remarks>
    /// 此接口使用通讯录应用Token<br />
    /// 详情见: <see href="https://developer.work.weixin.qq.com/document/path/96067" />
    /// </remarks>
    /// <param name="request">请求参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkGetUserIdListResponse> GetUserIdListAsync(
        WeChatWorkGetUserIdListRequest request,
        CancellationToken cancellationToken = default);
}

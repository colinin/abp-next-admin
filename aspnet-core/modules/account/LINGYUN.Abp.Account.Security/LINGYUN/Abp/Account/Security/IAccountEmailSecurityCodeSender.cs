using System;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Account.Security;
/// <summary>
/// 邮件安全码发送接口
/// </summary>
public interface IAccountEmailSecurityCodeSender
{
    /// <summary>
    /// 发送邮件登录验证码
    /// </summary>
    /// <param name="code">验证码</param>
    /// <param name="userName">用户名</param>
    /// <param name="emailAddress">邮件地址</param>
    /// <returns></returns>
    Task SendLoginCodeAsync(
        string code,
        string userName,
        string emailAddress);
    /// <summary>
    /// 发送邮件确认链接
    /// </summary>
    /// <param name="userId">用户Id</param>
    /// <param name="userEmail">用户邮件</param>
    /// <param name="confirmToken">确认Token</param>
    /// <param name="appName">应用名称</param>
    /// <param name="returnUrl">回调路径</param>
    /// <param name="returnUrlHash">回调路径Hash</param>
    /// <param name="userTenantId">用户租户Id</param>
    /// <returns></returns>
    Task SendConfirmLinkAsync(
        Guid userId,
        string userEmail,
        string confirmToken,
        string appName,
        string returnUrl = null,
        string returnUrlHash = null,
        Guid? userTenantId = null
    );
}

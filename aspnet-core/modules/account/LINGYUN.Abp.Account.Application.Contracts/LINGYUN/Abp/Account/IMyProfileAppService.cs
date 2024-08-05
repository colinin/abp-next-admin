using LINGYUN.Abp.Identity;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.Account;

public interface IMyProfileAppService : IApplicationService
{
    /// <summary>
    /// 获取验证器信息
    /// </summary>
    /// <returns></returns>
    Task<AuthenticatorDto> GetAuthenticator();
    /// <summary>
    /// 验证验证器代码
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<AuthenticatorRecoveryCodeDto> VerifyAuthenticatorCode(VerifyAuthenticatorCodeInput input);
    /// <summary>
    /// 获取会话列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<PagedResultDto<IdentitySessionDto>> GetSessionsAsync(GetMySessionsInput input);
    /// <summary>
    /// 撤销会话
    /// </summary>
    /// <param name="sessionId">会话id</param>
    /// <returns></returns>
    Task RevokeSessionAsync(string sessionId);
    /// <summary>
    /// 重置验证器
    /// </summary>
    /// <returns></returns>
    Task ResetAuthenticator();
    /// <summary>
    /// 获取二次认证状态
    /// </summary>
    /// <returns></returns>
    Task<TwoFactorEnabledDto> GetTwoFactorEnabledAsync();
    /// <summary>
    /// 改变二次认证
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task ChangeTwoFactorEnabledAsync(TwoFactorEnabledDto input);
    /// <summary>
    /// 发送改变手机号验证码
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task SendChangePhoneNumberCodeAsync(SendChangePhoneNumberCodeInput input);
    /// <summary>
    /// 改变手机绑定
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <remarks>
    /// 需二次认证,主要是为了无法用到重定向页面修改相关信息的地方（点名微信小程序）
    /// </remarks>
    Task ChangePhoneNumberAsync(ChangePhoneNumberInput input);
    /// <summary>
    /// 发送确认邮件验证码
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task SendEmailConfirmLinkAsync(SendEmailConfirmCodeDto input);
    /// <summary>
    /// 确认邮件地址
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task ConfirmEmailAsync(ConfirmEmailInput input);
}

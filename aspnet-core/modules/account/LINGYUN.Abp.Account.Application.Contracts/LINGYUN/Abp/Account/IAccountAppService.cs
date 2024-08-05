using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.Account;

public interface IAccountAppService : IApplicationService
{
    /// <summary>
    /// 通过手机号注册用户账户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task RegisterAsync(PhoneRegisterDto input);
    /// <summary>
    /// 通过微信小程序注册用户账户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task RegisterAsync(WeChatRegisterDto input);
    /// <summary>
    /// 通过手机号重置用户密码
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task ResetPasswordAsync(PhoneResetPasswordDto input);
    /// <summary>
    /// 发送手机注册验证码短信
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task SendPhoneRegisterCodeAsync(SendPhoneRegisterCodeDto input);
    /// <summary>
    /// 发送手机登录验证码短信
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task SendPhoneSigninCodeAsync(SendPhoneSigninCodeDto input);
    /// <summary>
    /// 发送邮件登录验证码
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task SendEmailSigninCodeAsync(SendEmailSigninCodeDto input);
    /// <summary>
    /// 发送手机重置密码验证码短信
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task SendPhoneResetPasswordCodeAsync(SendPhoneResetPasswordCodeDto input);
    /// <summary>
    /// 获取用户二次认证提供者列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<ListResultDto<NameValue>> GetTwoFactorProvidersAsync(GetTwoFactorProvidersInput input);
}

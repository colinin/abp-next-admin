using Asp.Versioning;
using LINGYUN.Abp.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.Account;

[RemoteService(Name = AccountRemoteServiceConsts.RemoteServiceName)]
[Area("account")]
[ControllerName("Profile")]
[Route("/api/account/my-profile")]
public class MyProfileController : AbpControllerBase, IMyProfileAppService
{
    protected IMyProfileAppService MyProfileAppService { get; }

    public MyProfileController(
        IMyProfileAppService myProfileAppService)
    {
        MyProfileAppService = myProfileAppService;
    }

    [HttpGet]
    [Route("sessions")]
    public virtual Task<PagedResultDto<IdentitySessionDto>> GetSessionsAsync(GetMySessionsInput input)
    {
        return MyProfileAppService.GetSessionsAsync(input);
    }

    [HttpDelete]
    [Route("sessions/{sessionId}/revoke")]
    public virtual Task RevokeSessionAsync(string sessionId)
    {
        return MyProfileAppService.RevokeSessionAsync(sessionId);
    }

    [HttpGet]
    [Route("two-factor")]
    public virtual Task<TwoFactorEnabledDto> GetTwoFactorEnabledAsync()
    {
        return MyProfileAppService.GetTwoFactorEnabledAsync();
    }

    [HttpPut]
    [Route("change-two-factor")]
    public virtual Task ChangeTwoFactorEnabledAsync(TwoFactorEnabledDto input)
    {
        return MyProfileAppService.ChangeTwoFactorEnabledAsync(input);
    }

    [HttpPost]
    [Route("send-phone-number-change-code")]
    public virtual Task SendChangePhoneNumberCodeAsync(SendChangePhoneNumberCodeInput input)
    {
        return MyProfileAppService.SendChangePhoneNumberCodeAsync(input);
    }

    [HttpPut]
    [Route("change-phone-number")]
    public virtual Task ChangePhoneNumberAsync(ChangePhoneNumberInput input)
    {
        return MyProfileAppService.ChangePhoneNumberAsync(input);
    }

    [HttpPost]
    [Route("send-email-confirm-link")]
    public virtual Task SendEmailConfirmLinkAsync(SendEmailConfirmCodeDto input)
    {
        return MyProfileAppService.SendEmailConfirmLinkAsync(input);
    }

    [HttpPut]
    [Route("confirm-email")]
    public virtual Task ConfirmEmailAsync(ConfirmEmailInput input)
    {
        return MyProfileAppService.ConfirmEmailAsync(input);
    }

    [HttpGet]
    [Route("authenticator")]
    public virtual Task<AuthenticatorDto> GetAuthenticator()
    {
        return MyProfileAppService.GetAuthenticator();
    }

    [HttpPost]
    [Route("verify-authenticator-code")]
    public virtual Task<AuthenticatorRecoveryCodeDto> VerifyAuthenticatorCode(VerifyAuthenticatorCodeInput input)
    {
        return MyProfileAppService.VerifyAuthenticatorCode(input);
    }

    [HttpPost]
    [Route("reset-authenticator")]
    public virtual Task ResetAuthenticator()
    {
        return MyProfileAppService.ResetAuthenticator();
    }
}

using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.Account
{
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
        [Route("two-factor")]
        public async virtual Task<TwoFactorEnabledDto> GetTwoFactorEnabledAsync()
        {
            return await MyProfileAppService.GetTwoFactorEnabledAsync();
        }

        [HttpPut]
        [Route("change-two-factor")]
        public async virtual Task ChangeTwoFactorEnabledAsync(TwoFactorEnabledDto input)
        {
            await MyProfileAppService.ChangeTwoFactorEnabledAsync(input);
        }

        [HttpPost]
        [Route("send-phone-number-change-code")]
        public async virtual Task SendChangePhoneNumberCodeAsync(SendChangePhoneNumberCodeInput input)
        {
            await MyProfileAppService.SendChangePhoneNumberCodeAsync(input);
        }

        [HttpPut]
        [Route("change-phone-number")]
        public async virtual Task ChangePhoneNumberAsync(ChangePhoneNumberInput input)
        {
            await MyProfileAppService.ChangePhoneNumberAsync(input);
        }

        [HttpPost]
        [Route("send-email-confirm-link")]
        public async virtual Task SendEmailConfirmLinkAsync(SendEmailConfirmCodeDto input)
        {
            await MyProfileAppService.SendEmailConfirmLinkAsync(input);
        }

        [HttpPut]
        [Route("confirm-email")]
        public async virtual Task ConfirmEmailAsync(ConfirmEmailInput input)
        {
            await MyProfileAppService.ConfirmEmailAsync(input);
        }

        [HttpGet]
        [Route("authenticator")]
        public async virtual Task<AuthenticatorDto> GetAuthenticator()
        {
            return await MyProfileAppService.GetAuthenticator();
        }

        [HttpPost]
        [Route("verify-authenticator-code")]
        public async virtual Task<AuthenticatorRecoveryCodeDto> VerifyAuthenticatorCode(VerifyAuthenticatorCodeInput input)
        {
            return await MyProfileAppService.VerifyAuthenticatorCode(input);
        }

        [HttpPost]
        [Route("reset-authenticator")]
        public async virtual Task ResetAuthenticator()
        {
            await MyProfileAppService.ResetAuthenticator();
        }
    }
}

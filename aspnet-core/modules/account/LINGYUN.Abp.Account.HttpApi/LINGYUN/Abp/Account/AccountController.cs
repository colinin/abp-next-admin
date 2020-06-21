using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Identity;

namespace LINGYUN.Abp.Account
{
    [RemoteService(Name = AccountRemoteServiceConsts.RemoteServiceName)]
    [Area("account")]
    [Route("api/account")]
    public class AccountController : AbpController, IAccountAppService
    {
        protected IAccountAppService AccountAppService { get; }
        public AccountController(
            IAccountAppService accountAppService)
        {
            AccountAppService = accountAppService;
        }

        [HttpPost]
        [Route("wechat/register")]
        public virtual async Task<IdentityUserDto> RegisterAsync(WeChatRegisterDto input)
        {
            return await AccountAppService.RegisterAsync(input);
        }

        [HttpPost]
        [Route("phone/register")]
        public virtual async Task<IdentityUserDto> RegisterAsync(PhoneNumberRegisterDto input)
        {
            return await AccountAppService.RegisterAsync(input);
        }

        [HttpPost]
        [Route("phone/verify")]
        public virtual async Task VerifyPhoneNumberAsync(VerifyDto input)
        {
            await AccountAppService.VerifyPhoneNumberAsync(input);
        }

        [HttpPut]
        [Route("phone/reset-password")]
        public virtual async Task ResetPasswordAsync(PasswordResetDto passwordReset)
        {
            await AccountAppService.ResetPasswordAsync(passwordReset);
        }
    }
}

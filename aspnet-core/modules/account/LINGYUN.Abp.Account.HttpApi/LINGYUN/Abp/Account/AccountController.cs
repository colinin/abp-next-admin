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
    [Route("api/account/phone")]
    public class AccountController : AbpController, IAccountAppService
    {
        protected IAccountAppService AccountAppService { get; }
        public AccountController(
            IAccountAppService accountAppService)
        {
            AccountAppService = accountAppService;
        }

        [HttpPost]
        [Route("register")]
        public virtual async Task<IdentityUserDto> RegisterAsync(RegisterVerifyDto input)
        {
            return await AccountAppService.RegisterAsync(input);
        }

        [HttpPost]
        [Route("verify")]
        public virtual async Task VerifyPhoneNumberAsync(VerifyDto input)
        {
            await AccountAppService.VerifyPhoneNumberAsync(input);
        }
    }
}

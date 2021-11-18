using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Identity;

namespace LINGYUN.Abp.Identity
{
    [RemoteService(Name = IdentityRemoteServiceConsts.RemoteServiceName)]
    [Area("identity")]
    [ControllerName("Profile")]
    [Route("/api/identity/my-profile")]
    public class MyProfileController : AbpController, IMyProfileAppService
    {
        protected IMyProfileAppService MyProfileAppService { get; }

        public MyProfileController(
            IMyProfileAppService myProfileAppService)
        {
            MyProfileAppService = myProfileAppService;
        }

        [HttpPut]
        [Route("/claims")]
        public virtual async Task SetClaimAsync(IdentityUserClaimSetDto input)
        {
            await MyProfileAppService.SetClaimAsync(input);
        }

        [HttpGet]
        [Route("two-factor")]
        public virtual async Task<TwoFactorEnabledDto> GetTwoFactorEnabledAsync()
        {
            return await MyProfileAppService.GetTwoFactorEnabledAsync();
        }

        [HttpPut]
        [Route("change-two-factor")]
        public virtual async Task ChangeTwoFactorEnabledAsync(TwoFactorEnabledDto input)
        {
            await MyProfileAppService.ChangeTwoFactorEnabledAsync(input);
        }

        [HttpPut]
        [Route("send-phone-number-change-code")]
        public virtual async Task SendChangePhoneNumberCodeAsync(SendChangePhoneNumberCodeDto input)
        {
            await MyProfileAppService.SendChangePhoneNumberCodeAsync(input);
        }

        [HttpPut]
        [Route("change-phone-number")]
        public virtual async Task ChangePhoneNumberAsync(ChangePhoneNumberDto input)
        {
            await MyProfileAppService.ChangePhoneNumberAsync(input);
        }
    }
}

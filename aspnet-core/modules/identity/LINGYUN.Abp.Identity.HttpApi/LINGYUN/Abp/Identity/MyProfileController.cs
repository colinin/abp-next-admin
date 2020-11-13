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
        [Route("change-two-factor")]
        public virtual async Task ChangeTwoFactorEnabledAsync(IdentityUserTwoFactorEnabledDto input)
        {
            await MyProfileAppService.ChangeTwoFactorEnabledAsync(input);
        }
    }
}

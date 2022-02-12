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
    [Route("/api/account/my-claim")]
    public class MyClaimController : AbpController, IMyClaimAppService
    {
        private readonly IMyClaimAppService _service;

        public MyClaimController(
            IMyClaimAppService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("change-avatar")]
        public virtual async Task ChangeAvatarAsync(ChangeAvatarInput input)
        {
            await _service.ChangeAvatarAsync(input);
        }
    }
}

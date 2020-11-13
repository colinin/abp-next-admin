using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace LINGYUN.Abp.Identity
{
    [Authorize]
    public class MyProfileAppService : IdentityAppServiceBase, IMyProfileAppService
    {
        protected IdentityUserManager UserManager { get; }
        protected IIdentityUserRepository UserRepository { get; }

        public MyProfileAppService(
            IdentityUserManager userManager,
            IIdentityUserRepository userRepository)
        {
            UserManager = userManager;
            UserRepository = userRepository;
        }

        public virtual async Task ChangeTwoFactorEnabledAsync(IdentityUserTwoFactorEnabledDto input)
        {
            var user = await UserManager.GetByIdAsync(CurrentUser.GetId());

            (await UserManager.SetTwoFactorEnabledWithAccountConfirmedAsync(user, input.Enabled)).CheckErrors();

            await CurrentUnitOfWork.SaveChangesAsync();
        }
    }
}

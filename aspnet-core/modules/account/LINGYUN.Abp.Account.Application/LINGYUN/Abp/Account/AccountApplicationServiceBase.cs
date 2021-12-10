using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Volo.Abp.Account.Localization;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace LINGYUN.Abp.Account
{
    public abstract class AccountApplicationServiceBase : ApplicationService
    {
        protected IOptions<IdentityOptions> IdentityOptions => LazyServiceProvider.LazyGetRequiredService<IOptions<IdentityOptions>>();
        protected IdentityUserStore UserStore => LazyServiceProvider.LazyGetRequiredService<IdentityUserStore>();
        protected IdentityUserManager UserManager => LazyServiceProvider.LazyGetRequiredService<IdentityUserManager>();

        protected AccountApplicationServiceBase()
        {
            LocalizationResource = typeof(AccountResource);
        }

        protected virtual async Task<IdentityUser> GetCurrentUserAsync()
        {
            await IdentityOptions.SetAsync();

            return await UserManager.GetByIdAsync(CurrentUser.GetId());
        }
    }
}

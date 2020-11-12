using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;

namespace LINGYUN.Abp.WeChat.Authorization
{
    [Dependency(ServiceLifetime.Transient, ReplaceServices = true)]
    [ExposeServices(typeof(IUserWeChatOpenIdFinder))]
    public class UserWeChatOpenIdFinder : IUserWeChatOpenIdFinder
    {
        protected IdentityUserManager UserManager { get; }

        public UserWeChatOpenIdFinder(
            IdentityUserManager userManager)
        {
            UserManager = userManager;
        }

        public virtual async Task<string> FindByUserIdAsync(Guid userId)
        {
            var user = await UserManager.FindByIdAsync(userId.ToString());

            return GetUserOpenIdOrNull(user);
        }

        public virtual async Task<string> FindByUserNameAsync(string userName)
        {
            var user = await UserManager.FindByNameAsync(userName);

            return GetUserOpenIdOrNull(user);
        }

        protected string GetUserOpenIdOrNull(IdentityUser user)
        {
            // 微信扩展登录后openid存储在Login中
            var userLogin = user?.Logins
                .Where(login => login.LoginProvider == AbpWeChatAuthorizationConsts.ProviderKey)
                .FirstOrDefault();

            return userLogin?.ProviderKey;
        }
    }
}

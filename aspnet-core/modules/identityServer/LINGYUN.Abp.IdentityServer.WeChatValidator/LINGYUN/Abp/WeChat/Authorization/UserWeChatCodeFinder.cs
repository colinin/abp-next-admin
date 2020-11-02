using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;

namespace LINGYUN.Abp.WeChat.Authorization
{
    // TODO: 真正的项目需要扩展Abp框架实体来关联微信
    [Dependency(ServiceLifetime.Transient, ReplaceServices = true)]
    [ExposeServices(typeof(IUserWeChatCodeFinder))]
    public class UserWeChatCodeFinder : IUserWeChatCodeFinder
    {
        protected IdentityUserManager UserManager { get; }

        public UserWeChatCodeFinder(
            IdentityUserManager userManager)
        {
            UserManager = userManager;
        }

        public virtual async Task<string> FindByUserIdAsync(Guid userId)
        {
            var user = await UserManager.FindByIdAsync(userId.ToString());

            var weChatCodeToken = user?.FindToken(WeChatAuthorizationConsts.ProviderKey, WeChatAuthorizationConsts.WeCahtCodeKey);

            return weChatCodeToken?.Value ?? userId.ToString();
        }

        public virtual async Task<string> FindByUserNameAsync(string userName)
        {
            var user = await UserManager.FindByNameAsync(userName);

            var weChatCodeToken = user?.FindToken(WeChatAuthorizationConsts.ProviderKey, WeChatAuthorizationConsts.WeCahtCodeKey);

            return weChatCodeToken?.Value ?? userName;
        }
    }
}

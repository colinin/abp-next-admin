using LINGYUN.Abp.WeChat.Work;
using LINGYUN.Abp.WeChat.Work.Authorize;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;

namespace LINGYUN.Abp.Identity.WeChat.Work
{
    [Dependency(ServiceLifetime.Transient, ReplaceServices = true)]
    [ExposeServices(typeof(IWeChatWorkInternalUserFinder))]
    public class WeChatWorkInternalUserFinder : IWeChatWorkInternalUserFinder
    {
        protected IdentityUserManager UserManager { get; }

        public WeChatWorkInternalUserFinder(
            IdentityUserManager userManager)
        {
            UserManager = userManager;
        }

        protected string GetUserOpenIdOrNull(IdentityUser user, string provider)
        {
            // 微信扩展登录后openid存储在Login中
            var userLogin = user?.Logins
                .Where(login => login.LoginProvider == provider)
                .FirstOrDefault();

            return userLogin?.ProviderKey;
        }

        public async virtual Task<string> FindUserIdentifierAsync(string agentId, Guid userId, CancellationToken cancellationToken = default)
        {
            var user = await UserManager.FindByIdAsync(userId.ToString());

            return GetUserOpenIdOrNull(user, AbpWeChatWorkGlobalConsts.ProviderName);
        }

        public async virtual Task<List<string>> FindUserIdentifierListAsync(string agentId, IEnumerable<Guid> userIdList, CancellationToken cancellationToken = default)
        {
            var userIdentifiers = new List<string>();
            foreach (var userId in userIdList)
            {
                var user = await UserManager.FindByIdAsync(userId.ToString());
                var weChatWorkUserId = GetUserOpenIdOrNull(user, AbpWeChatWorkGlobalConsts.ProviderName);
                if (!weChatWorkUserId.IsNullOrWhiteSpace())
                {
                    userIdentifiers.Add(weChatWorkUserId);
                }
            }

            return userIdentifiers;
        }
    }
}

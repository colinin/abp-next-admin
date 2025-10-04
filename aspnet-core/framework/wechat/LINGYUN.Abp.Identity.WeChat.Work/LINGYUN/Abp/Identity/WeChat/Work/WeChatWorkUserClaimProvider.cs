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
using Volo.Abp.Uow;

namespace LINGYUN.Abp.Identity.WeChat.Work;

[Dependency(ServiceLifetime.Transient, ReplaceServices = true)]
[ExposeServices(typeof(IWeChatWorkUserClaimProvider))]
public class WeChatWorkUserClaimProvider : IWeChatWorkUserClaimProvider
{
    protected IdentityUserManager UserManager { get; }

    public WeChatWorkUserClaimProvider(
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

    public async virtual Task<string> FindUserIdentifierAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await UserManager.FindByIdAsync(userId.ToString());

        return GetUserOpenIdOrNull(user, AbpWeChatWorkGlobalConsts.ProviderName);
    }

    public async virtual Task<List<string>> FindUserIdentifierListAsync(IEnumerable<Guid> userIdList, CancellationToken cancellationToken = default)
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

    [UnitOfWork]
    public async virtual Task BindUserAsync(
        Guid userId,
        string weChatUserId,
        CancellationToken cancellationToken = default)
    {
        var user = await UserManager.GetByIdAsync(userId);
        var existsWeChatUserId = GetUserOpenIdOrNull(user, AbpWeChatWorkGlobalConsts.ProviderName);
        if (!existsWeChatUserId.IsNullOrWhiteSpace())
        {
            user.RemoveLogin(AbpWeChatWorkGlobalConsts.ProviderName, existsWeChatUserId);
        }
        user.AddLogin(new Microsoft.AspNetCore.Identity.UserLoginInfo(
            AbpWeChatWorkGlobalConsts.ProviderName,
            weChatUserId,
            AbpWeChatWorkGlobalConsts.DisplayName));
    }
}

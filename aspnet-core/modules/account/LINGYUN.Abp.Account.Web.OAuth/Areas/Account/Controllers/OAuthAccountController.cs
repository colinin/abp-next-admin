using AspNet.Security.OAuth.WorkWeixin;
using LINGYUN.Abp.WeChat.Work.Authorize;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Identity;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.Users;

namespace LINGYUN.Abp.Account.Web.OAuth.Areas.Account.Controllers;

[Controller]
[Area(AccountRemoteServiceConsts.ModuleName)]
[Route($"api/{AccountRemoteServiceConsts.ModuleName}/oauth")]
[RemoteService(Name = AccountRemoteServiceConsts.RemoteServiceName)]
public class OAuthAccountController : AbpController
{
    protected IWeChatWorkUserFinder WeChatWorkUserFinder => LazyServiceProvider.LazyGetRequiredService<IWeChatWorkUserFinder>();
    protected AbpSignInManager SignInManager => LazyServiceProvider.LazyGetRequiredService<AbpSignInManager>();
    protected IdentityUserManager UserManager => LazyServiceProvider.LazyGetRequiredService<IdentityUserManager>();

    [HttpPost]
    [Authorize]
    [Route("work-weixin/bind")]
    public virtual async Task WorkWeixinLoginBindAsync(string code)
    {
        var workWeixinUser = await WeChatWorkUserFinder.GetUserInfoAsync(code);
        var currentUser = await UserManager.GetByIdAsync(CurrentUser.GetId());
        var userLogins = await UserManager.GetLoginsAsync(currentUser);
        var workWexinLogin = userLogins.FirstOrDefault(x => x.LoginProvider == WorkWeixinAuthenticationDefaults.AuthenticationScheme);
        if (workWexinLogin != null)
        {
            (await UserManager.RemoveLoginAsync(currentUser, workWexinLogin.LoginProvider, workWexinLogin.ProviderKey)).CheckErrors();
        }
        (await UserManager.AddLoginAsync(
            currentUser,
            new UserLoginInfo(
                WorkWeixinAuthenticationDefaults.AuthenticationScheme,
                workWeixinUser.UserId,
                WorkWeixinAuthenticationDefaults.DisplayName))).CheckErrors();
    }
}

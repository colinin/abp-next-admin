using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace LINGYUN.Abp.Account.WeChat.Work.Controllers;

[Authorize]
public class WeChatWorkAccountController : AbpControllerBase
{
    private readonly IdentityUserManager identityUserManager;
    /// <summary>
    /// 绑定用户
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    public async virtual Task BindAsync(string code)
    {
        var user = await identityUserManager.GetByIdAsync(CurrentUser.GetId());

    }
}

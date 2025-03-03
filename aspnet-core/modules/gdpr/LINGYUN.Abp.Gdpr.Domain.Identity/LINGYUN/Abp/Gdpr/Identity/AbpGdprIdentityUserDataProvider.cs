using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.ChangeTracking;
using Volo.Abp.Gdpr;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace LINGYUN.Abp.Gdpr.Identity;

/// <summary>
/// 身份标识模块用户数据提供者
/// </summary>
public class AbpGdprIdentityUserDataProvider: GdprUserDataProviderBase
{
    public const string ProviderName = "Identity";

    public override string Name => ProviderName;

    public async override Task DeleteAsync(GdprDeleteUserDataContext context)
    {
        var identityUserManager = context.ServiceProvider.GetRequiredService<IdentityUserManager>();

        var identityUser = await identityUserManager.GetByIdAsync(context.UserId);

        identityUser.Name = "";
        identityUser.Surname = "";

        // TODO: 应设置为空字符串, 但是abp框架不允许Email为空
        (await identityUserManager.SetEmailAsync(identityUser, $"{identityUser.UserName}@abp.io")).CheckErrors();
        (await identityUserManager.SetPhoneNumberAsync(identityUser, "")).CheckErrors();

        // 清理身份标识
        var userClaims = await identityUserManager.GetClaimsAsync(identityUser);
        (await identityUserManager.RemoveClaimsAsync(identityUser, userClaims)).CheckErrors();

        // TODO: 是否需要清理第三方关联账户?
        //var userLogins = await identityUserManager.GetLoginsAsync(identityUser);
        //foreach (var userLogin in userLogins)
        //{
        //    (await identityUserManager.RemoveLoginAsync(identityUser, userLogin.LoginProvider, userLogin.ProviderKey)).CheckErrors();
        //}

        (await identityUserManager.UpdateAsync(identityUser)).CheckErrors();
    }

    [DisableEntityChangeTracking]
    public async override Task PorepareAsync(GdprPrepareUserDataContext context)
    {
        var identityUserManager = context.ServiceProvider.GetRequiredService<IdentityUserManager>();
        var identityUser = await identityUserManager.GetByIdAsync(context.UserId);

        var gdprDataInfo = new GdprDataInfo
        {
            { nameof(UserData.UserName), identityUser.UserName },
            { nameof(UserData.Name), identityUser.Name },
            { nameof(UserData.Surname), identityUser.Surname },
            { nameof(UserData.Email), identityUser.Email },
            { nameof(UserData.PhoneNumber), identityUser.PhoneNumber },
            { nameof(IHasCreationTime.CreationTime), identityUser.CreationTime.ToString("yyyy-MM-dd HH:mm:ss") },
        };

        foreach (var identityUserClaim in identityUser.Claims)
        {
            gdprDataInfo.Add(identityUserClaim.ClaimType, identityUserClaim.ClaimValue);
        }

        await DispatchPrepareUserDataAsync(context, gdprDataInfo);
    }
}
